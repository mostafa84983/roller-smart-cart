using SmartCart.Application.Common;
using SmartCart.Application.Dto;
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
/*        Task<GenericResult<PaginatedResult<ProductDto>>> GetPaginatedProductsOfOrder(int orderId, int page, int pageSize, int userClaims , string role);
*/        Task<GenericResult<ProductDto>> GetProductByCode(int productCode);
        Task<GenericResult<ProductDto>> GetProductById(int productId);
    }
}
