using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SmartCart.API.Hubs;
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
        private readonly IHubContext<CartHub> _hubContext ;

        public ProductController(IProductService productService , IHubContext<CartHub> hubContext)
        {
            _productService = productService;
            _hubContext = hubContext;
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
            var (products, total) = result.Value;
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(new { products, total });
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
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto productDto)
        {
            var result = await _productService.CreateProduct(productDto);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }


        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDto productDto)
        {
            var result = await _productService.UpdateProduct(productDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok();
        }

        [HttpPost("Add/Product")]
        public async Task<IActionResult> AddProductToCart ([FromBody] ProductRequest productRequest) 
        {
            var userId = GetUserId();
            var result = await _productService.AddProductservice(productRequest, userId);

           
            if (result.IsSuccess)
            {
                var (product, total) = result.Value;

                await _hubContext.Clients.Group(productRequest.CartId.ToString())
               .SendAsync("ProductAdded", new { product, total });

                return Ok(new { product, total });

            }
            
            else
                return BadRequest(result.ErrorMessage);
        }

        [HttpPost("Remove/Product")]
        public async Task<IActionResult> RemoveProductFromCart([FromBody] ProductRequest productRequest)
        {
            var userId = GetUserId();
            var result = await _productService.RemoveProductservice(productRequest, userId);
            
            if (result.IsSuccess)
            {
                var (product, total) = result.Value;

                await _hubContext.Clients.Group(productRequest.CartId.ToString())
               .SendAsync("ProductRemoved", new { product, total });

                return Ok(new { product, total });
            }

            else
                return BadRequest(result.ErrorMessage);
        }


        [HttpPost("Failed/Product")]
        public async Task<IActionResult> FailedProductDetection(string cartId)
        {

            await _hubContext.Clients.Group(cartId)
               .SendAsync("FailedProductDetection");

            return Ok();

        }

        [HttpPost("OCR")]
        public async Task<IActionResult> OpenOCR(string cartId)
        {

            //Send to external API
            //using (var httpClient = new HttpClient())
            //{
            //    var response = await httpClient.PostAsJsonAsync("http://host.docker.internal:5050/open-OCR", true);

            //}

            return Ok();

        }

        [HttpPost("REDO")]
        public async Task<IActionResult> REDO(string cartId)
        {
            // Send to external API
            //using (var httpClient = new HttpClient())
            //{
            //    var response = await httpClient.PostAsJsonAsync("http://host.docker.internal:5050/REDO", true);

            //}

            return Ok();

        }


        [HttpPost("Detection")]
        public async Task<IActionResult> ProductDetection([FromBody] ProductRequest productRequest)
        {
            var result = await _productService.GetProductByCode(productRequest.ProductCode);
            if(result.IsSuccess)
            {
                await _hubContext.Clients.Group(productRequest.CartId.ToString())
               .SendAsync("ProductDetection" , result.Value);
                return Ok(result.Value);
            }

            return BadRequest(result.ErrorMessage);
           

        }



    }
}
