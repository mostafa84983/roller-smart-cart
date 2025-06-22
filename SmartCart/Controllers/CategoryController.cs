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
                return NotFound(result.ErrorMessage);

            return Ok(result.Value);
        }


        [HttpGet("offers")]
        public async Task<IActionResult> GetCategoriesWithOffers()
        {
            var result = await _categoryService.GetCategoriesWithOffers();
            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return Ok(result.Value);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createdCategoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _categoryService.CreateCategory(createdCategoryDto);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto updatedCategoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _categoryService.UpdateCategory(updatedCategoryDto);
            if(!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

           return Ok(result);
        }
    }
}
