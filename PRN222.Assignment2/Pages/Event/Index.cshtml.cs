using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using PRN222.Assignment1.Business.DTOs;
using PRN222.Assignment1.Business.Services;
using PRN222.Assignment1.Data.Pagination;
using PRN222.Assignment2.Hubs;

namespace PRN222.Assignment2.Pages.Event
{
    public class IndexModel : PageModel
    {
        private readonly IEventService _eventService;
        private readonly IEventCategoryService _eventCategoryService;
        private readonly IAttendeeService _attendeeService;
        private readonly IHubContext<EventHub> _eventHubContext;

        public IndexModel(IEventService eventService, IEventCategoryService eventCategoryService, IAttendeeService attendeeService, IHubContext<EventHub> eventHubContext)
        {
            _eventService = eventService;
            _eventCategoryService = eventCategoryService;
            _attendeeService = attendeeService;
            _eventHubContext = eventHubContext;
        }

        public PaginatedList<EventDto> Events { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 6;

        public async Task OnGetAsync()
        {
            dynamic events = await LoadPaginatedEvents(SearchTerm, CategoryId, PageIndex, PageSize);
            Events = events;
            await LoadEventCategories();
            ViewData["selectedCategory"] = CategoryId;
            if (User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ViewData["attendedEvents"] = await _eventService.GetAttendedEventsAsync(userId);
                ViewData["canAttendEvents"] = await _eventService.GetCanAttendEventsAsync(userId);
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var deletedEvent = await _eventService.DeleteEventAsync(id.Value);
                await _eventHubContext.Clients.All.SendAsync("ReceiveEventDeleted", id.Value);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToPage("./Index");
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
