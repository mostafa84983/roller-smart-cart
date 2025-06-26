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
        Task<IEnumerable<OrderProduct>> GetPaginatedProductsOfOrder(int orderId, int page, int pageSize);
        Task<Product> GetProductByCode(int productCode);
        Task<bool> AddOfferToProduct(int productId, decimal offerPercentage);
        Task<bool> RemoveOfferFromProduct(int productId);
        Task<bool> SoftDeleteProduct(int productId);
        Task<bool> RestoreProduct(int productId);
        Task<bool> IsProductNameTaken(string productName, int? productId);


    }
}
