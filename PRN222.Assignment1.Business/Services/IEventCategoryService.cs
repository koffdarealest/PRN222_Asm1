using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222.Assignment1.Business.DTOs;

namespace PRN222.Assignment1.Business.Services
{
    public interface IEventCategoryService
    {
        Task<ICollection<EventCategoryDto>> GetAllEventCategoriesAsync();
        ICollection<EventCategoryDto> GetAllEventCategories();
    }
}
