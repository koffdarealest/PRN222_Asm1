using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PRN222.Assignment1.Business.DTOs;
using PRN222.Assignment1.Data.Repositories;

namespace PRN222.Assignment1.Business.Services.Implements
{
    public class EventCategoryService : IEventCategoryService
    {
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IMapper _mapper;

        public EventCategoryService(IEventCategoryRepository eventCategoryRepository, IMapper mapper)
        {
            _eventCategoryRepository = eventCategoryRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<EventCategoryDto>> GetAllEventCategoriesAsync()
        {
            dynamic result = await _eventCategoryRepository.GetCategoriesAsync();
            return _mapper.Map<ICollection<EventCategoryDto>>(result);
        }

        public ICollection<EventCategoryDto> GetAllEventCategories()
        {
            dynamic result = _eventCategoryRepository.GetCategories();
            return _mapper.Map<ICollection<EventCategoryDto>>(result);
        }
    }
}
