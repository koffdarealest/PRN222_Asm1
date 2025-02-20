using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment1.Data.Context;
using PRN222.Assignment1.Data.Models;
using PRN222.Assignment1.Data.Pagination;

namespace PRN222.Assignment1.Data.Repositories.Implements
{
    public class EventRepository : IEventRepository
    {
        private readonly Prn222asm1Context _context;

        public EventRepository(Prn222asm1Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Attendees)
                .ToListAsync();
        }

        public async Task<Event> CreateEventAsync(Event e)
        {
            await _context.Events.AddAsync(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<Event> DeleteEventAsync(int eventId)
        {
            Event? e = await _context.Events.FindAsync(eventId);
            if (e != null)
            {
                _context.Events.Remove(e);
                await _context.SaveChangesAsync();
            }

            return e;
        }

        public async Task<IEnumerable<Event>> GetEventsByCategoryIdAsync(int categoryId)
        {
            return await _context.Events.Where(e => e.CategoryId == categoryId).ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int eventId)
        {
            Event? e = await _context.Events.Include(e => e.Category).Where(e => e.EventId == eventId)
                .FirstOrDefaultAsync();
            return e;
        }

        public async Task<Event> GetEventByIdAsync(int eventId, bool tracking)
        {
            if (tracking)
            {
                return await _context.Events
                    .Include(e => e.Category)
                    .Include(e => e.Attendees)
                    .Where(e => e.EventId == eventId)
                    .FirstOrDefaultAsync();
            }
            else
            {
                return await _context.Events
                    .AsNoTracking()
                    .Include(e => e.Category)
                    .Include(e => e.Attendees)
                    .Where(e => e.EventId == eventId)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<Event>> GetEventsByKeywordAsync(string keyword)
        {
            return await _context.Events.Where(e => e.Title.Contains(keyword)).ToListAsync();
        }

        public async Task<Event> UpdateEventAsync(Event e)
        {
            _context.Events.Update(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<PaginatedList<Event>> GetPaginatedAttendedEventsAsync(int userId, int pageIndex, int pageSize)
        {
            //return await _context.Events
            //    .Include(e => e.Attendees)
            //    .Where(e => e.Attendees.Any(a => a.UserId == userId))
            //    .ToListAsync();
            return await PaginatedList<Event>.CreateAsync(_context.Events
                .Include(e => e.Category)
                .Include(e => e.Attendees)
                .Where(e => e.Attendees.Any(a => a.UserId == userId))
                .OrderBy(e => e.StartTime)
                .AsNoTracking(), pageIndex, pageSize);
        }

        public async Task<PaginatedList<Event>> GetPaginatedCanAttendEventsAsync(int userId, int pageIndex, int pageSize)
        {
            //return await _context.Events
            //    .Include(e => e.Attendees)
            //    .Where(e => e.Attendees.All(a => a.UserId != userId)).ToListAsync();
            return await PaginatedList<Event>.CreateAsync(_context.Events
                .Include(e => e.Category)
                .Include(e => e.Attendees)
                .Where(e => e.Attendees.All(a => a.UserId != userId))
                .OrderBy(e => e.StartTime)
                .AsNoTracking(), pageIndex, pageSize);
        }

        public async Task<PaginatedList<Event>> GetPaginatedEventsAsync(int pageIndex, int pageSize)
        {
            return await PaginatedList<Event>.CreateAsync(_context.Events
                .Include(e => e.Category)
                .Include(e => e.Attendees)
                .OrderBy(e => e.StartTime)
                .AsNoTracking(), pageIndex, pageSize);
        }

        public async Task<PaginatedList<Event>> GetPaginatedEventsAsync(string? searchTerm, int? categoryId, int pageIndex, int pageSize)
        {
            IQueryable<Event> query = _context.Events
                .Include(e => e.Category)
                .Include(e => e.Attendees)
                .OrderBy(e => e.StartTime)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e => e.Title.Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(e => e.CategoryId == categoryId);
            }

            return await PaginatedList<Event>.CreateAsync(query, pageIndex, pageSize);
        }

        public async Task<ICollection<Event>> GetAttendedEventsAsync(int userId)
        {
            return await _context.Events
                .Include(e => e.Attendees)
                .Include(e => e.Category)
                .Where(e => e.Attendees.Any(a => a.UserId == userId))
                .OrderBy(e => e.StartTime)
                .ToListAsync();
        }

        public async Task<ICollection<Event>> GetCanAttendEventsAsync(int userId)
        {
            return await _context.Events
                .Include(e => e.Attendees)
                .Include(e => e.Category)
                .Where(e => e.Attendees.All(a => a.UserId != userId))
                .OrderBy(e => e.StartTime)
                .ToListAsync();
        }

        public async Task<ICollection<Event>> GetTopTrendingEventsAsync()
        {
            return await _context.Events
                .Include(e => e.Attendees)
                .OrderByDescending(e => e.Attendees.Count)
                .Take(3)
                .ToListAsync();
        }

        public async Task<int> GetTotalEventsAsync()
        {
            return await _context.Events.CountAsync();
        }

        public async Task<Dictionary<string, int>> GetTotalEventsByCategoriesAsync()
        {
            return await _context.Events
                .Include(e => e.Category)
                .GroupBy(e => e.Category.CategoryName)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.Category, v => v.Count);
        }
    }
}
