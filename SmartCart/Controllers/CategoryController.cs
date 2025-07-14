using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCart.Application.Dto.Category;
using SmartCart.Application.Interfaces;

namespace SmartCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetAllPaginatedCategories(int page=1, int pageSize=10)
        {
            var result = await _categoryService.GetAllPaginatedCategories(page, pageSize);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage ?? "An unexpected error occurred");

            return Ok(result.Value);
        }


        [HttpGet("offers")]
        public async Task<IActionResult> GetPaginatedCategoriesWithOffers(int page =1 , int pageSize =10)
        {
            var result = await _categoryService.GetPaginatedCategoriesWithOffers(page , pageSize);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage ?? "An unexpected error occurred");

            return Ok(result.Value);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryDto createdCategoryDto)
        {
            var result = await _categoryService.CreateCategory(createdCategoryDto);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }


        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategoryDto updatedCategoryDto)
        {
            var result = await _categoryService.UpdateCategory(updatedCategoryDto);
            if(!result.IsSuccess)
                return BadRequest(result.ErrorMessage);
            
           return Ok();
        }
    }
}
