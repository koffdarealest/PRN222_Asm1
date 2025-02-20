using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222.Assignment1.Data.Models;
using PRN222.Assignment1.Data.Pagination;

namespace PRN222.Assignment1.Data.Repositories
{
    public interface IEventRepository 
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(int eventId);
        Task<Event> GetEventByIdAsync(int eventId, bool tracking);
        Task<Event> CreateEventAsync(Event e);
        Task<Event> UpdateEventAsync(Event e);
        Task<Event> DeleteEventAsync(int eventId);
        Task<IEnumerable<Event>> GetEventsByCategoryIdAsync(int categoryId);
        Task<PaginatedList<Event>> GetPaginatedCanAttendEventsAsync(int userId, int pageIndex, int pageSize);
        Task<IEnumerable<Event>> GetEventsByKeywordAsync(string keyword);
        Task<PaginatedList<Event>> GetPaginatedAttendedEventsAsync(int userId, int pageIndex, int pageSize);
        Task<PaginatedList<Event>> GetPaginatedEventsAsync(int pageIndex, int pageSize);
        Task<PaginatedList<Event>> GetPaginatedEventsAsync(string? searchTerm, int? categoryId, int pageIndex, int pageSize);
        Task<ICollection<Event>> GetAttendedEventsAsync(int userId);
        Task<ICollection<Event>> GetCanAttendEventsAsync(int userId);
        Task<ICollection<Event>> GetTopTrendingEventsAsync();
        Task<int> GetTotalEventsAsync();
        Task<Dictionary<string, int>> GetTotalEventsByCategoriesAsync();
    }
}
