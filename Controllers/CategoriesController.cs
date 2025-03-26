using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.DTOs.Categories;
using Planify_BackEnd.Services.Categories;

namespace Planify_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet("{campusId}")]
        public IActionResult GetCategoryByCampusId(int campusId)
        {
            try
            {
                var response = _categoryService.GetCategoryByCampusId(campusId);
                return Ok(response);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Campus Manager")]
        public async Task<IActionResult> CreateCategory(CategoryDTO categoryDTO)
        {
            try
            {
                var response = await _categoryService.CreateCategory(categoryDTO);
                return Ok(response);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Authorize(Roles = "Campus Manager")]
        public async Task<IActionResult> UpdateCategory(CategoryDTO categoryDTO)
        {
            try
            {
                var response = await _categoryService.UpdateCategory(categoryDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{categoryId}")]
        [Authorize(Roles = "Campus Manager")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            try
            {
                var response = await _categoryService.DeleteCategory(categoryId);
                if (!response) return BadRequest("Cannot delete category!");
                return Ok(response);
            }
            catch (NullReferenceException nEx)
            {
                return NotFound("Cannot found any category with id " + categoryId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
