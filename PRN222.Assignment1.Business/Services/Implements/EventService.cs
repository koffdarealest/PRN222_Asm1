using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PRN222.Assignment1.Business.DTOs;
using PRN222.Assignment1.Data.Models;
using PRN222.Assignment1.Data.Pagination;
using PRN222.Assignment1.Data.Repositories;

namespace PRN222.Assignment1.Business.Services.Implements
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<EventDto>> GetAllEventsAsync()
        {
            dynamic events = await _eventRepository.GetAllEventsAsync();
            return _mapper.Map<ICollection<EventDto>>(events);
        }

        public async Task<EventDto> CreateEventAsync(EventDto eventDto)
        {
            dynamic result = await _eventRepository.CreateEventAsync(_mapper.Map<Event>(eventDto));
            return _mapper.Map<EventDto>(result);
        }

        public async Task<EventDto> GetEventByIdAsync(int id)
        {
            dynamic result = await _eventRepository.GetEventByIdAsync(id);
            return _mapper.Map<EventDto>(result);
        }

        public async Task<EventDto> GetEventByIdAsync(int id, bool tracking)
        {
            dynamic result = await _eventRepository.GetEventByIdAsync(id, tracking);
            return _mapper.Map<EventDto>(result);
        }

        public async Task<EventDto> UpdateEventAsync(EventDto eventDto)
        {
            dynamic result = await _eventRepository.UpdateEventAsync(_mapper.Map<Event>(eventDto));
            return _mapper.Map<EventDto>(result);
        }

        public async Task<EventDto> DeleteEventAsync(int id)
        {
            dynamic result = await _eventRepository.DeleteEventAsync(id);
            return _mapper.Map<EventDto>(result);
        }

        public async Task<PaginatedList<EventDto>> GetPaginatedAttendedEventsAsync(int userId, int pageIndex, int pageSize)
        {
            dynamic result = await _eventRepository.GetPaginatedAttendedEventsAsync(userId, pageIndex, pageSize);
            var eventDtos = _mapper.Map<List<EventDto>>(result);
            return new PaginatedList<EventDto>(eventDtos, result.TotalCount, pageIndex, pageSize);
        }

        public async Task<PaginatedList<EventDto>> GetPaginatedCanAttendEventsAsync(int userId, int pageIndex, int pageSize)
        {
            dynamic result = await _eventRepository.GetPaginatedCanAttendEventsAsync(userId, pageIndex, pageSize);
            var eventDtos = _mapper.Map<List<EventDto>>(result);
            return new PaginatedList<EventDto>(eventDtos, result.TotalCount, pageIndex, pageSize);
        }

        public async Task<PaginatedList<EventDto>> GetPaginatedEventsAsync(int pageIndex, int pageSize)
        {
            dynamic result = await _eventRepository.GetPaginatedEventsAsync(pageIndex, pageSize);
            var eventDtos = _mapper.Map<List<EventDto>>(result);
            return new PaginatedList<EventDto>(eventDtos, result.TotalCount, pageIndex, pageSize);
        }

        public async Task<PaginatedList<EventDto>> GetPaginatedEventsAsync(string? searchTerm, int? categoryId, int pageIndex, int pageSize)
        {
            dynamic result = await _eventRepository.GetPaginatedEventsAsync(searchTerm, categoryId, pageIndex, pageSize);
            var eventDtos = _mapper.Map<List<EventDto>>(result);
            return new PaginatedList<EventDto>(eventDtos, result.TotalCount, pageIndex, pageSize);
        }

        public async Task<ICollection<EventDto>> GetAttendedEventsAsync(int userId)
        {
            return _mapper.Map<ICollection<EventDto>>(await _eventRepository.GetAttendedEventsAsync(userId));
        }

        public async Task<ICollection<EventDto>> GetCanAttendEventsAsync(int userId)
        {
            return _mapper.Map<ICollection<EventDto>>(await _eventRepository.GetCanAttendEventsAsync(userId));
        }

        public async Task<ICollection<EventDto>> GetTopTrendingEventsAsync()
        {
            return _mapper.Map<ICollection<EventDto>>(await _eventRepository.GetTopTrendingEventsAsync());
        }

        public async Task<int> GetTotalEventsAsync()
        {
            return await _eventRepository.GetTotalEventsAsync();
        }

        public async Task<Dictionary<string, int>> GetTotalEventsByCategoriesAsync()
        {
            return await _eventRepository.GetTotalEventsByCategoriesAsync();
        }
    }
}
