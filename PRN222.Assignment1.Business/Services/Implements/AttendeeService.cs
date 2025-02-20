using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PRN222.Assignment1.Business.DTOs;
using PRN222.Assignment1.Data.Models;
using PRN222.Assignment1.Data.Repositories;

namespace PRN222.Assignment1.Business.Services.Implements
{
    public class AttendeeService : IAttendeeService
    {
        private readonly IAttendeeRepository _attendeeRepository;
        private readonly IMapper _mapper;

        public AttendeeService(IAttendeeRepository attendeeRepository, IMapper mapper)
        {
            _attendeeRepository = attendeeRepository;
            _mapper = mapper;
        }

        public async Task<AttendeeDto> CreateAttendeeAsync(AttendeeDto attendeeDto)
        {
            return _mapper.Map<AttendeeDto>(await _attendeeRepository.CreateAttendeeAsync(_mapper.Map<Attendee>(attendeeDto)));
        }

        public Task<AttendeeDto> DeleteAttendeeAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<AttendeeDto>> GetAllAttendeesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AttendeeDto> GetAttendeeByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AttendeeDto> UpdateAttendeeAsync(AttendeeDto attendeeDto)
        {
            throw new NotImplementedException();
        }

        public async Task<Boolean> IsUserAttendedEventAsync(int userId, int eventId)
        {
            return await _attendeeRepository.IsUserAttendedEventAsync(userId, eventId);
        }

        public async Task<Dictionary<string, int>> GetTotalAttendeesByCategoriesAsync()
        {
            return await _attendeeRepository.GetTotalAttendeesByCategoriesAsync();
        }

        public async Task<int> GetTotalAttendeesAsync()
        {
            return await _attendeeRepository.GetTotalAttendeesAsync();
        }
    }
}
