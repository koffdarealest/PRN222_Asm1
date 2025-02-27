using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Assignment1.Business.Services;
using PRN222.Assignment2.Models;

namespace PRN222.Assignment2.Pages.Report
{
    [Authorize(Roles = "ADMIN")]
    public class IndexModel : PageModel
    {
        private readonly IEventService _eventService;
        private readonly IEventCategoryService _eventCategoryService;
        private readonly IAttendeeService _attendeeService;

        public IndexModel(IEventService eventService, IEventCategoryService eventCategoryService, IAttendeeService attendeeService)
        {
            _eventService = eventService;
            _eventCategoryService = eventCategoryService;
            _attendeeService = attendeeService;
        }

        [BindProperty] public ReportViewModel ReportViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var totalEvents = await _eventService.GetTotalEventsAsync();
            var totalAttendees = await _attendeeService.GetTotalAttendeesAsync();
            var totalAttendeesByCategory = await _attendeeService.GetTotalAttendeesByCategoriesAsync();
            var topTrendingEvents = await _eventService.GetTopTrendingEventsAsync();
            var totalEventsByCategory = await _eventService.GetTotalEventsByCategoriesAsync();
            List<EventTrend> eventTrends = new List<EventTrend>();
            if (topTrendingEvents != null)
            {
                foreach (var e in topTrendingEvents)
                {
                    eventTrends.Add(new EventTrend
                    {
                        EventId = e.EventId,
                        Title = e.Title,
                        TotalAttendees = e.Attendees.Count
                    });
                }
            }
            ReportViewModel.TotalEvents = totalEvents;
            ReportViewModel.TotalAttendees = totalAttendees;
            ReportViewModel.TotalAttendeesByCategory = totalAttendeesByCategory;
            ReportViewModel.EventTrends = eventTrends;
            ReportViewModel.TotalEventsByCategory = totalEventsByCategory;
            return Page();
        }
    }
}
