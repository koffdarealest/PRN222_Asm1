using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment1.Data.Context;
using PRN222.Assignment1.Data.Models;

namespace PRN222.Assignment1.Data.Repositories.Implements
{
    public class AttendeeRepository : IAttendeeRepository
    {
        private readonly Prn222asm1Context _context;

        public AttendeeRepository(Prn222asm1Context context)
        {
            _context = context;
        }

        public async Task<Attendee> CreateAttendeeAsync(Attendee attendee)
        {
            await _context.Attendees.AddAsync(attendee);
            await _context.SaveChangesAsync();
            return attendee;
        }

        public async Task<Attendee> DeleteAttendeeAsync(int attendeeId)
        {
            Attendee? attendee = await _context.Attendees.FindAsync(attendeeId);
            if (attendee != null)
            {
                _context.Attendees.Remove(attendee);
                await _context.SaveChangesAsync();
            }
            return attendee;
        }

        public async Task<Attendee> GetAttendeeByIdAsync(int attendeeId)
        {
            Attendee? attendee = await _context.Attendees.FindAsync(attendeeId);
            return attendee;
        }

        public async Task<IEnumerable<Attendee>> GetAttendeesByEventIdAsync(int eventId)
        {
            return await _context.Attendees.Where(a => a.EventId == eventId).ToListAsync();
        }

        public async Task<IEnumerable<Attendee>> GetAttendeesByUserIdAsync(int userId)
        {
            return await _context.Attendees.Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<Attendee> UpdateAttendeeAsync(Attendee attendee)
        {
            Attendee? a = await _context.Attendees.FindAsync(attendee.AttendeeId);
            if (a != null)
            {
                a.Name = attendee.Name;
                a.Email = attendee.Email;
                a.RegistrationTime = attendee.RegistrationTime;
                await _context.SaveChangesAsync();
                
            }
            return a;
        }

        public async Task<Boolean> IsUserAttendedEventAsync(int userId, int eventId)
        {
            Attendee? a = await _context.Attendees.Where(a => a.EventId == eventId && a.UserId == userId).FirstOrDefaultAsync();
            return a != null;
        }

        public async Task<Dictionary<string, int>> GetTotalAttendeesByCategoriesAsync()
        {
            //return await _context.Events
            //    .GroupBy(e => e.Category.CategoryName)
            //    .Select(group => new
            //    {
            //        CategoryName = group.Key,
            //        AttendeeCount = group.Sum(e => e.Attendees.Count)
            //    })
            //    .ToDictionaryAsync(e => e.CategoryName, e => e.AttendeeCount);
            return await _context.Attendees
                .Include(a => a.Event)
                .ThenInclude(e => e.Category)
                .GroupBy(a => a.Event.Category.CategoryName)
                .Select(group => new
                {
                    CategoryName = group.Key,
                    AttendeeCount = group.Count()
                })
                .ToDictionaryAsync(e => e.CategoryName, e => e.AttendeeCount);
        }

        public async Task<int> GetTotalAttendeesAsync()
        {
            return await _context.Attendees.CountAsync();
        }
    }
}
