using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCart.Application.Dto.Category;
using SmartCart.Application.Dto.Product;
using SmartCart.Application.Interfaces;
using SmartCart.Application.Services;
using SmartCart.Domain.Enums;

namespace SmartCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetPaginatedProductsInCategory(int categoryId, int page = 1, int pageSize = 10)
        {
            var result = await _productService.GetPaginatedProductsInCategory(categoryId, page, pageSize);
            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpGet("category/{categoryId}/offers")]
        public async Task<IActionResult> GetPaginatedProductsWithOfferInCategory(int categoryId, int page = 1, int pageSize = 10)
        {
            var result = await _productService.GetPaginatedProductsWithOfferInCategory(categoryId, page, pageSize);
            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return Ok(result.Value);
        }


        [HttpGet("order/{orderId}/products")]
        [Authorize]
        public async Task<IActionResult> GetPaginatedProductsOfOrder(int orderId, int page = 1, int pageSize = 10)
        {
            var userIdClaims = GetUserId();
            var role = GetUserRole();

            if (!Enum.TryParse<RoleEnum>(role, ignoreCase: true, out var roleEnum))
                return BadRequest("Invalid role in token");
            

            var result = await _productService.GetPaginatedProductsOfOrder(orderId, page, pageSize, userIdClaims, roleEnum);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }


        [HttpGet("code/{productCode}")]
        public async Task<IActionResult> GetProductByCode(int productCode)
        {
            var result = await _productService.GetProductByCode(productCode);
            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var result = await _productService.GetProductById(productId);
            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SoftDeleteProduct(int productId)
        {
            var result = await _productService.SoftDeleteProduct(productId);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok();

        }

        [HttpPut("{productId}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreProduct(int productId)
        {
            var result = await _productService.RestoreProduct(productId);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPut("add-offer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOfferToProduct([FromBody] AddOfferDto productDto)
        {
            var result = await _productService.AddOfferToProduct(productDto);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPut("remove-offer")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> RemoveOfferFromProduct([FromBody] RemoveOfferDto productDto)
        {
            var result = await _productService.RemoveOfferFromProduct(productDto);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            var result = await _productService.CreateProduct(productDto);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto productDto)
        {

            var result = await _productService.UpdateProduct(productDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok();
        }


    }
}
