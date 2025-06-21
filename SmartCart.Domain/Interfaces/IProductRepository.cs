using SmartCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Domain.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<(IEnumerable<Product> Data, int TotalCount)> GetPaginatedProductsInCategory(int categoryId,int page, int pageSize);
        Task<(IEnumerable<Product> Data, int TotalCount)> GetPaginatedProductsWithOfferInCategory(int categoryId, int page, int pageSize);
        Task<(IEnumerable<Product> Data, int TotalCount)> GetPaginatedProductsOfOrder(int orderId, int page, int pageSize);
        Task<Product> GetProductByCode(int productCode);

    }
}
