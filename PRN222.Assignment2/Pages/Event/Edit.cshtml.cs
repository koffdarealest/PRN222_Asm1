using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using PRN222.Assignment1.Business.DTOs;
using PRN222.Assignment1.Business.Services;
using PRN222.Assignment2.Hubs;
using PRN222.Assignment2.Models;

namespace PRN222.Assignment2.Pages.Event
{
    public class EditModel : PageModel
    {
        private readonly IEventService _eventService;
        private readonly IEventCategoryService _eventCategoryService;
        private readonly IHubContext<EventHub> _eventHubContext;

        public EditModel(IEventService eventService, IEventCategoryService eventCategoryService, IHubContext<EventHub> eventHubContext)
        {
            _eventService = eventService;
            _eventCategoryService = eventCategoryService;
            _eventHubContext = eventHubContext;
        }

        [BindProperty] public EventCreateViewModel EventCreateViewModel { get; set; } = new();
        [BindProperty] public List<SelectListItem> EventCategories { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var e = await _eventService.GetEventByIdAsync(id.Value);

            EventCreateViewModel.Title = e.Title;
            EventCreateViewModel.Description = e.Description;
            EventCreateViewModel.StartTime = e.StartTime;
            EventCreateViewModel.EndTime = e.EndTime;
            EventCreateViewModel.Location = e.Location;
            EventCreateViewModel.CategoryId = e.CategoryId;

            await LoadEventCategories();

            //if (eventCategories.IsNullOrEmpty())
            //{
            //    ViewData["CategoryId"] = new SelectList(Enumerable.Empty<object>(), "CategoryId", "CategoryName");
            //}
            //else
            //{
            //    ViewData["CategoryId"] = new SelectList(eventCategories, "CategoryId", "CategoryName");
            //}

            

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                await LoadEventCategories();
                return Page();
            }

            if (id == null)
            {
                return NotFound();
            }

            var eventDto = new EventDto
            {
                EventId = id.Value,
                Title = EventCreateViewModel.Title,
                Description = EventCreateViewModel.Description,
                StartTime = EventCreateViewModel.StartTime,
                EndTime = EventCreateViewModel.EndTime,
                Location = EventCreateViewModel.Location,
                CategoryId = EventCreateViewModel.CategoryId
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
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await LoadEventCategories();
                return Page();
            }
        }

        private async Task<ICollection<EventCategoryDto>> LoadEventCategories()
        {
            var eventsCategories = await _eventCategoryService.GetAllEventCategoriesAsync();
            EventCategories = eventsCategories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();
            return eventsCategories;
        }

    }
}
