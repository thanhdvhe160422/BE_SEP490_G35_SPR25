using Planify_BackEnd.DTOs.Categories;

namespace Planify_BackEnd.Services.Categories
{
    public interface ICategoryService
    {
        public IEnumerable<CategoryViewModel> GetCategoryByCampusId(int campusId);
    }
}
