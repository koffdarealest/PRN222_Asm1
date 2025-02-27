using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using PRN222.Assignment1.Business.Services;
using PRN222.Assignment2.Hubs;
using PRN222.Assignment2.Models;
using System.Security.Claims;
using PRN222.Assignment1.Business.DTOs;
using Microsoft.Extensions.Logging;

namespace PRN222.Assignment2.Pages.Event
{
    [Authorize(Roles = "USER")]
    public class AttendModel : PageModel
    {
        private readonly IEventService _eventService;
        private readonly IEventCategoryService _eventCategoryService;
        private readonly IAttendeeService _attendeeService;
        private readonly IHubContext<EventHub> _eventHubContext;

        public AttendModel(IEventService eventService, IEventCategoryService eventCategoryService, IAttendeeService attendeeService, IHubContext<EventHub> eventHubContext)
        {
            _eventService = eventService;
            _eventCategoryService = eventCategoryService;
            _attendeeService = attendeeService;
            _eventHubContext = eventHubContext;
        }

        [BindProperty] public AttendCreateViewModel AttendCreateViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? eventId)
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

            AttendCreateViewModel.EventId = e.EventId;
            AttendCreateViewModel.EventTitle = e.Title;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var eventId = AttendCreateViewModel.EventId;
            if (!ModelState.IsValid)
            {
                return Redirect($"Event/Attend?eventid={eventId}");
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            bool isAttended = await _attendeeService.IsUserAttendedEventAsync(userId, eventId);
            
            if (isAttended)
            {
                ModelState.AddModelError(string.Empty, "You have already attended this event");
                return Redirect($"Event/Attend?eventid={eventId}");
            }

            AttendeeDto attendeeDto = new AttendeeDto
            {
                EventId = eventId,
                UserId = userId,
                Name = AttendCreateViewModel.Name,
                Email = AttendCreateViewModel.Email,
                RegistrationTime = DateTime.Now
            };

            try
            {
                await _attendeeService.CreateAttendeeAsync(attendeeDto);
                var e = await _eventService.GetEventByIdAsync(eventId, tracking: false);
                await _eventHubContext.Clients.All.SendAsync("ReceiveAttendeeCountUpdated", eventId, e.Attendees.Count);
                return RedirectToPage("./Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }
        }
    }
}
