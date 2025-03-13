using Planify_BackEnd.DTOs.Categories;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.Categories
{
    public interface ICategoryRepository
    {
        Task<CategoryEvent> GetByIdAsync(int CategoryEventId);
        IEnumerable<CategoryEvent> GetCategoryByCampusId(int campusId);
    }
}
