using Planify_BackEnd.DTOs.Categories;
using Planify_BackEnd.Repositories.Categories;

namespace Planify_BackEnd.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public IEnumerable<CategoryViewModel> GetCategoryByCampusId(int campusId)
        {
            try
            {
                var list = _categoryRepository.GetCategoryByCampusId(campusId);
                if (list== null) { throw new Exception(); }
                List<CategoryViewModel> listVM =  list.Select(c => new CategoryViewModel
                {
                    Id= c.Id,
                    CampusId= c.CampusId,
                    CategoryEventName = c.CategoryEventName,
                    Status = c.Status,
                }).ToList();
                return listVM;
            }catch(Exception ex)
            {
                Console.WriteLine("category service - get category by campus: " + ex.Message);
                return new List<CategoryViewModel>();
            }
        }

        public async Task<CategoryViewModel> GetCategoryByName(string categoryName, int campusId)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByName(categoryName, campusId);
                CategoryViewModel categoryVM = new CategoryViewModel
                {
                    Id = category.Id,
                    CampusId = category.CampusId,
                    CategoryEventName = category.CategoryEventName,
                    Status = category.Status,
                };
                return categoryVM;
            }
            catch
            {
                return new CategoryViewModel();
            }
        }
    }
}
