using PRN222.Assignment1.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Assignment1.Data.Repositories
{
    public interface IEventCategoryRepository
    {
        Task<EventCategory> GetCategoryByIdAsync(int categoryId);
        Task<EventCategory> CreateCategoryAsync(EventCategory category);
        Task<EventCategory> UpdateCategoryAsync(EventCategory category);
        Task<EventCategory> DeleteCategoryAsync(int categoryId);
        Task<IEnumerable<EventCategory>> GetCategoriesAsync();
        IEnumerable<EventCategory> GetCategories(); 
    }
}
