using Microsoft.EntityFrameworkCore;
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
    }
}
