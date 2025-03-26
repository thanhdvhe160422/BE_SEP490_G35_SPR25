using Planify_BackEnd.DTOs.Categories;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Services.Categories
{
    public interface ICategoryService
    {
        IEnumerable<CategoryViewModel> GetCategoryByCampusId(int campusId);
        Task<CategoryViewModel> GetCategoryByName(string categoryName, int campusId);
        Task<CategoryViewModel> CreateCategory(CategoryDTO categoryDTO);
        Task<CategoryViewModel> UpdateCategory(CategoryDTO categoryDTO);
        Task<bool> DeleteCategory(int categoryId);
    }
}
