using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN222.Assignment1.Business.DTOs;
using PRN222.Assignment1.Business.Services;
using PRN222.Assignment1.Models;

namespace PRN222.Assignment1.Controllers
{
    public class ReportController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IEventCategoryService _eventCategoryService;
        private readonly IAttendeeService _attendeeService;

        public ReportController(IEventService eventService, IEventCategoryService eventCategoryService, IAttendeeService attendeeService)
        {
            _eventService = eventService;
            _eventCategoryService = eventCategoryService;
            _attendeeService = attendeeService;
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index()
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
            ReportViewModel reportViewModel = new ReportViewModel
            {
                TotalEvents = totalEvents,
                TotalAttendees = totalAttendees,
                TotalAttendeesByCategory = totalAttendeesByCategory,
                EventTrends = eventTrends,
                TotalEventsByCategory = totalEventsByCategory
            };

            return View(reportViewModel);
        }
    }
}
