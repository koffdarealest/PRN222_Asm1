using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222.Assignment1.Business.DTOs;

namespace PRN222.Assignment1.Business.Services
{
    public interface IAttendeeService
    {
        Task<ICollection<AttendeeDto>> GetAllAttendeesAsync();
        Task<AttendeeDto> CreateAttendeeAsync(AttendeeDto attendeeDto);
        Task<AttendeeDto> GetAttendeeByIdAsync(int id);
        Task<AttendeeDto> UpdateAttendeeAsync(AttendeeDto attendeeDto);
        Task<AttendeeDto> DeleteAttendeeAsync(int id);

        Task<Boolean> IsUserAttendedEventAsync(int userId, int eventId);
        Task<int> GetTotalAttendeesAsync();
        Task<Dictionary<string, int>> GetTotalAttendeesByCategoriesAsync();
    }
}
