using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222.Assignment1.Business.DTOs;
using PRN222.Assignment1.Data.Pagination;

namespace PRN222.Assignment1.Business.Services
{
    public interface IEventService
    {
        Task<ICollection<EventDto>> GetAllEventsAsync();
        Task<EventDto> CreateEventAsync(EventDto eventDto);
        Task<EventDto> GetEventByIdAsync(int id);
        Task<EventDto> GetEventByIdAsync(int id, bool tracking);
        Task<EventDto> UpdateEventAsync(EventDto eventDto);
        Task<EventDto> DeleteEventAsync(int id);
        Task<PaginatedList<EventDto>> GetPaginatedAttendedEventsAsync(int userId, int pageIndex, int pageSize);
        Task<PaginatedList<EventDto>> GetPaginatedCanAttendEventsAsync(int userId, int pageIndex, int pageSize);
        Task<PaginatedList<EventDto>> GetPaginatedEventsAsync(int pageIndex, int pageSize);
        Task<PaginatedList<EventDto>> GetPaginatedEventsAsync(string? searchTerm, int? categoryId, int pageIndex, int pageSize);
        Task<ICollection<EventDto>> GetAttendedEventsAsync(int userId);
        Task<ICollection<EventDto>> GetCanAttendEventsAsync(int userId);
        Task <int> GetTotalEventsAsync();
        Task<ICollection<EventDto>> GetTopTrendingEventsAsync();
        Task<Dictionary<string, int>> GetTotalEventsByCategoriesAsync();
    }
}
