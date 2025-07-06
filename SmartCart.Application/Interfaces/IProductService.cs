using SmartCart.Application.Common;
using SmartCart.Application.Dto;
using SmartCart.Application.Dto.Product;
using SmartCart.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Interfaces
{
    public interface IProductService
    {
        Task<GenericResult<PaginatedResult<ProductDto>>> GetPaginatedProductsInCategory(int categoryId, int page, int pageSize);
        Task<GenericResult<PaginatedResult<ProductDto>>> GetPaginatedProductsWithOfferInCategory(int categoryId, int page, int pageSize);
        Task<GenericResult<(IEnumerable<ProductInOrderDto>, decimal)>> GetPaginatedProductsOfOrder(int orderId, int page, int pageSize, int userClaims, RoleEnum role);
        Task<GenericResult<ProductDto>> GetProductByCode(int productCode);
        Task<GenericResult<ProductDto>> GetProductById(int productId);
        Task<Result> CreateProduct(CreateProductDto product);
        Task<Result> UpdateProduct(UpdateProductDto productDto);
        Task<Result> AddOfferToProduct(AddOfferDto productDto);
        Task<Result> RemoveOfferFromProduct(RemoveOfferDto productDto);
        Task<Result> SoftDeleteProduct(int productId);
        Task<Result> RestoreProduct(int productId);
        Task<GenericResult<(ProductAddOrRemoveDto, decimal)>> AddProductservice(ProductRequest productRequest , int userId);
        Task<GenericResult<(ProductAddOrRemoveDto, decimal)>> RemoveProductservice(ProductRequest productRequest, int userId);
        Task<GenericResult<decimal>> AddProductToOrder(int productCode, int orderId);
        Task<GenericResult<decimal>> RemoveProductFromOrder(int productCode, int orderId);

    }
}
