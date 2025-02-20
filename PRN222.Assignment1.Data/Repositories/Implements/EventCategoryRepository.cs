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
    public class EventCategoryRepository : IEventCategoryRepository
    {
        private readonly Prn222asm1Context _context;

        public EventCategoryRepository(Prn222asm1Context context)
        {
            _context = context;
        }

        public async Task<EventCategory> CreateCategoryAsync(EventCategory category)
        {
            await _context.EventCategories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<EventCategory> DeleteCategoryAsync(int categoryId)
        {
            EventCategory? category = await _context.EventCategories.FindAsync(categoryId);
            if (category != null)
            {
                _context.EventCategories.Remove(category);
                await _context.SaveChangesAsync();
                
            }
            return category;
        }

        public async Task<EventCategory> GetCategoryByIdAsync(int categoryId)
        {
            EventCategory? category = await _context.EventCategories.FindAsync(categoryId);
            return category;
        }

        public async Task<IEnumerable<EventCategory>> GetCategoriesAsync()
        {
            return await _context.EventCategories.ToListAsync();
        }

        public async Task<EventCategory> UpdateCategoryAsync(EventCategory category)
        {
            EventCategory? categoryInDb = await _context.EventCategories.FindAsync(category.CategoryId);
            if (categoryInDb != null)
            {
                categoryInDb.CategoryName = category.CategoryName;
                await _context.SaveChangesAsync();
            }
            return categoryInDb;
        }

        public IEnumerable<EventCategory> GetCategories()
        {
            return _context.EventCategories.ToList();
        }
    }
}
