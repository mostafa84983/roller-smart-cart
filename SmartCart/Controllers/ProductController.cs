using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCart.Application.Interfaces;

namespace SmartCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
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


        /*        [HttpGet("order/{orderId}")]
                  [Authorize]
                public async Task<IActionResult> GetPaginatedProductsOfOrder(int orderId, int page = 1, int pageSize = 10)
                {

                }*/


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
    }
}
