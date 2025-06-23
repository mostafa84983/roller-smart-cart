using SmartCart.Application.Common;
using SmartCart.Application.Dto;
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
        Task<GenericResult<PaginatedResult<ProductDto>>> GetPaginatedProductsOfOrder(int orderId, int page, int pageSize, int userClaims, RoleEnum role);
        Task<GenericResult<ProductDto>> GetProductByCode(int productCode);
        Task<GenericResult<ProductDto>> GetProductById(int productId);
        Task<Result> CreateProduct (ProductDto product);
        Task<Result> UpdateProduct (ProductDto product);
        Task<Result> AddOfferToProduct(int productId, decimal offerPercentage);
        Task<Result> RemoveOfferFromProduct(int productId);
        Task<Result> SoftDeleteProduct(int productId);
        Task<Result> RestoreProduct(int productId);
    }
}
