using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using PRN222.Assignment1.Business.DTOs;
using PRN222.Assignment1.Business.Services;
using PRN222.Assignment2.Models;

namespace PRN222.Assignment2.Pages.Event
{
    [Authorize(Roles = "ADMIN")]
    public class CreateModel : PageModel
    {
        private readonly IEventService _eventService;
        private readonly IEventCategoryService _eventCategoryService;

        public CreateModel(IEventService eventService, IEventCategoryService eventCategoryService)
        {
            _eventService = eventService;
            _eventCategoryService = eventCategoryService;
        }

        [BindProperty] public EventCreateViewModel EventCreateViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var events = await LoadEventCategories();
            if (events.IsNullOrEmpty())
            {
                ViewData["CategoryId"] = new SelectList(Enumerable.Empty<object>(), "CategoryId", "CategoryName");
            }
            else
            {
                ViewData["CategoryId"] = new SelectList(events, "CategoryId", "CategoryName");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadEventCategories();
                return Page();
            }

            var eventDto = new EventDto
            {
                Title = EventCreateViewModel.Title,
                Description = EventCreateViewModel.Description,
                StartTime = EventCreateViewModel.StartTime,
                EndTime = EventCreateViewModel.EndTime,
                Location = EventCreateViewModel.Location,
                CategoryId = EventCreateViewModel.CategoryId
            };

            try
            {
                await _eventService.CreateEventAsync(eventDto);
                return RedirectToPage("Index");
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
            dynamic events = await _eventCategoryService.GetAllEventCategoriesAsync();
            return events;
        }
    }
}
