using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.SignalR;
using PRN222.Assignment1.Business.DTOs;
using PRN222.Assignment1.Business.Services;
using PRN222.Assignment1.Data.Models;
using PRN222.Assignment1.Data.Pagination;
using PRN222.Assignment1.Hubs;
using PRN222.Assignment1.Models;

namespace PRN222.Assignment1.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IEventCategoryService _eventCategoryService;
        private readonly IAttendeeService _attendeeService;
        private readonly IHubContext<EventHub> _eventHubContext;
        public EventController(IEventService eventService, IEventCategoryService eventCategoryService, IAttendeeService attendeeService, IHubContext<EventHub> eventHubContext)
        {
            _eventService = eventService;
            _eventCategoryService = eventCategoryService;
            _attendeeService = attendeeService;
            _eventHubContext = eventHubContext;
        }

        public async Task<IActionResult> Index(string? searchTerm, int? categoryId, int page = 1, int pageSize = 4)
        {
            dynamic events = await LoadPaginatedEvents(searchTerm, categoryId, page, pageSize);
            await LoadEventCategories();
            ViewData["searchTerm"] = searchTerm;
            ViewData["selectedCategory"] = categoryId;
            if (User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ViewData["attendedEvents"] = await _eventService.GetAttendedEventsAsync(userId);
                ViewData["canAttendEvents"] = await _eventService.GetCanAttendEventsAsync(userId);
            }
            return View(events);
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create()
        {
            await LoadEventCategories();
            return View(new EventCreateViewModel());
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventDto = await _eventService.GetEventByIdAsync(id.Value);
            if (eventDto == null)
            {
                return NotFound();
            }

            EventCreateViewModel eventCreateViewModel = new EventCreateViewModel
            {
                Title = eventDto.Title,
                Description = eventDto.Description,
                StartTime = eventDto.StartTime,
                EndTime = eventDto.EndTime,
                Location = eventDto.Location,
                CategoryId = eventDto.CategoryId
            };
            await LoadEventCategories();
            return View(eventCreateViewModel);
        }

        [Authorize(Roles= "ADMIN")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventDto = await _eventService.DeleteEventAsync(id.Value);
            if (eventDto == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Attend(int? eventId)
        {
            if (eventId == null)
            {
                return NotFound();
            }
            var e = await _eventService.GetEventByIdAsync(eventId.Value);
            if (e == null)
            {
                return NotFound();
            }
            AttendCreateViewModel attendCreateViewModel = new AttendCreateViewModel
            {
                EventTitle = e.Title,
            };
            TempData["EventId"] = eventId;
            return View(attendCreateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create(EventCreateViewModel eventCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                await LoadEventCategories();
                return View(eventCreateViewModel);
            }

            EventDto eventDto = new EventDto
            {
                Title = eventCreateViewModel.Title,
                Description = eventCreateViewModel.Description,
                StartTime = eventCreateViewModel.StartTime,
                EndTime = eventCreateViewModel.EndTime,
                Location = eventCreateViewModel.Location,
                CategoryId = eventCreateViewModel.CategoryId
            };

            try
            {
                await _eventService.CreateEventAsync(eventDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await LoadEventCategories();
                return View(eventCreateViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(int id, EventCreateViewModel eventCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                await LoadEventCategories();
                return View(eventCreateViewModel);
            }

            EventDto eventDto = new EventDto
            {
                EventId = id,
                Title = eventCreateViewModel.Title,
                Description = eventCreateViewModel.Description,
                StartTime = eventCreateViewModel.StartTime,
                EndTime = eventCreateViewModel.EndTime,
                Location = eventCreateViewModel.Location,
                CategoryId = eventCreateViewModel.CategoryId
            };

            try
            {
                var updatedEvent = await _eventService.UpdateEventAsync(eventDto);
                await _eventHubContext.Clients.All.SendAsync("ReceiveEventUpdated", 
                    updatedEvent.EventId,
                    updatedEvent.Title,
                    updatedEvent.Description ?? "",
                    updatedEvent.StartTime?.ToString("yyyy-MM-dd HH:mm") ?? "",
                    updatedEvent.EndTime?.ToString("yyyy-MM-dd HH:mm") ?? "",
                    updatedEvent.Location ?? "");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await LoadEventCategories();
                return View(eventCreateViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> Attend(AttendCreateViewModel attendCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Redirect($"Event/Attend?eventid={attendCreateViewModel.EventId}");
            }

            var eventId = (int)TempData["EventId"];
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            bool isAttended = await _attendeeService.IsUserAttendedEventAsync(userId, eventId);
            if (isAttended)
            {
                ModelState.AddModelError(string.Empty, "You have already attended this event");
                return View(attendCreateViewModel);
            }
            AttendeeDto attendeeDto = new AttendeeDto
            {
                EventId = eventId,
                UserId = userId,
                Name = attendCreateViewModel.Name,
                Email = attendCreateViewModel.Email,
                RegistrationTime = DateTime.Now
            };

            try
            {
                await _attendeeService.CreateAttendeeAsync(attendeeDto);
                var e = await _eventService.GetEventByIdAsync(eventId, tracking:false);
                await _eventHubContext.Clients.All.SendAsync("ReceiveAttendeeCountUpdated", eventId, e.Attendees.Count);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(attendCreateViewModel);
            }
        }

        private async Task<ICollection<EventCategoryDto>> LoadEventCategories()
        {
            dynamic events = await _eventCategoryService.GetAllEventCategoriesAsync();
            if (events == null)
            {
                ViewData["CategoryId"] = new SelectList(Enumerable.Empty<object>(), "CategoryId", "CategoryName");
            }
            else
            {
                ViewData["CategoryId"] = new SelectList(events, "CategoryId", "CategoryName");
            }
            return events;
        }

        private async Task<ICollection<EventDto>> LoadEvents()
        {
            return await _eventService.GetAllEventsAsync();
        }

        private async Task<PaginatedList<EventDto>> LoadPaginatedEvents(int pageIndex, int pageSize)
        {
            return await _eventService.GetPaginatedEventsAsync(pageIndex, pageSize);
        }

        private async Task<PaginatedList<EventDto>> LoadPaginatedEvents(string? searchTerm, int? categoryId, int pageIndex, int pageSize)
        {
            return await _eventService.GetPaginatedEventsAsync(searchTerm, categoryId, pageIndex, pageSize);
        }
    }
}
