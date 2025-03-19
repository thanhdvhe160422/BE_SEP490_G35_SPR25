using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.DTOs.Categories;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly PlanifyContext _context;
        public CategoryRepository(PlanifyContext context)
        {
            _context = context;
        }
        public async Task<CategoryEvent> GetByIdAsync(int CategoryEventId)
        {
            try {
                return await _context.CategoryEvents.FirstOrDefaultAsync(a => a.Id == CategoryEventId);
            } 
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }
        }

        public IEnumerable<CategoryEvent> GetCategoryByCampusId(int campusId)
        {
            try
            {
                var list = _context.CategoryEvents.Where(c=>c.CampusId==campusId&&c.Status==1).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine("category repository - get by campus id: " + ex.Message);
                return new List<CategoryEvent>();
            }
        }

        public async Task<CategoryEvent> GetCategoryByName(string categoryName, int campusId)
        {
            try
            {
                var category = await _context.CategoryEvents
                    .FirstOrDefaultAsync(c => c.CategoryEventName.Contains(categoryName) 
                    && c.CampusId == campusId
                    && c.Status == 1);
                return category;
            }
            catch
            {
                return new CategoryEvent();
            }
        }
    }
}
