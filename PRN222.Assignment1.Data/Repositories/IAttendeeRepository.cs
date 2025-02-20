using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222.Assignment1.Data.Models;

namespace PRN222.Assignment1.Data.Repositories
{
    public interface IAttendeeRepository
    {
        Task<Attendee> GetAttendeeByIdAsync(int attendeeId);
        Task<Attendee> CreateAttendeeAsync(Attendee attendee);
        Task<Attendee> UpdateAttendeeAsync(Attendee attendee);
        Task<Attendee> DeleteAttendeeAsync(int attendeeId);
        Task<IEnumerable<Attendee>> GetAttendeesByEventIdAsync(int eventId);
        Task<IEnumerable<Attendee>> GetAttendeesByUserIdAsync(int userId);
        Task<Boolean> IsUserAttendedEventAsync(int userId, int eventId);
        Task<Dictionary<string, int>> GetTotalAttendeesByCategoriesAsync();
        Task<int> GetTotalAttendeesAsync();
    }
}
