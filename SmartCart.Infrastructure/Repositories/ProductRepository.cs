using Microsoft.EntityFrameworkCore;
using SmartCart.Domain.Interfaces;
using SmartCart.Domain.Models;
using SmartCart.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product> , IProductRepository
    {
        public ProductRepository(DataContext Context) : base(Context)
        { 
        }

        public async Task<(IEnumerable<Product> Data, int TotalCount)> GetPaginatedProductsInCategory(int categoryId, int page, int pageSize)
        {
            var query = _context.Products.Where(p => p.CategoryId == categoryId && p.IsAvaiable && !p.IsDeleted && p.Quantity>0)
                                .OrderBy(p => p.ProductId)
                            .AsNoTracking();

            var totalCount = await query.CountAsync();
            var data = await query
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

            return (data, totalCount);
        }

        public async Task<(IEnumerable<Product> Data, int TotalCount)> GetPaginatedProductsWithOfferInCategory(int categoryId, int page, int pageSize)
        {
            var query = _context.Products.Where(p => p.CategoryId == categoryId && p.IsOffer && p.IsAvaiable && !p.IsDeleted && p.Quantity>0)
                            .OrderBy(p => p.ProductId)
                            .AsNoTracking();

            var totalCount = await query.CountAsync();
            var data = await query
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

            return (data, totalCount);
        }

        public async Task<IEnumerable<OrderProduct>> GetPaginatedProductsOfOrder(int orderId, int page, int pageSize)
        {
            var query = _context.OrderProducts
            .Where(op => op.OrderId == orderId)
            .Include(op => op.Product) 
            .AsNoTracking();

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderBy(op => op.ProductId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return data;
        }


        //public async Task<(IEnumerable<Product> Data, int TotalCount)> GetPaginatedProductsOfOrder(int orderId, int page, int pageSize)
        //{
        //    var query = _context.OrderProducts.Where(op => op.OrderId == orderId).Select(p => p.Product)
        //        .OrderBy(p => p.ProductId)
        //        .AsNoTracking();

        //    var totalCount = await query.CountAsync();

        //    var data = await query
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync();

        //    return (data, totalCount);
        //}



        public async Task<Product> GetProductByCode(int productCode)
        {
            return await _context.Products.Where(p => p.ProductCode == productCode && !p.IsDeleted && p.IsAvaiable && p.Quantity > 0)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SoftDeleteProduct(int productId)
        {
            var productToDelete = await _context.Products.FindAsync(productId);
            if (productToDelete == null || productToDelete.IsDeleted)
                return false;

            productToDelete.IsDeleted = true;
            return true;

        }

        public async Task<bool> RestoreProduct(int productId)
        {
            var productToRestore = await _context.Products.FindAsync(productId);
            if (productToRestore == null || !productToRestore.IsDeleted)
                return false;

            productToRestore.IsDeleted = false;
            return true;
        }

        public async Task<bool> AddOfferToProduct(int productId, decimal offerPercentage)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null || product.IsDeleted || !product.IsAvaiable || product.Quantity<= 0)
                return false;
            
            var category = await _context.Categories.FindAsync(product.CategoryId);
            if (category == null)
                return false; 

            product.OfferPercentage = offerPercentage;
            product.IsOffer = true;

            category.IsOffer = true;

            return true;
        }

        public async Task<bool> RemoveOfferFromProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null || product.IsDeleted || !product.IsOffer)
                return false;


            product.OfferPercentage = 0;
            product.IsOffer = false;

            var category = await _context.Categories.FindAsync(product.CategoryId);
            if (category == null)
                return false;

            bool noOtherProductsHaveOffer = !await _context.Products.AnyAsync(p =>
                                                 p.CategoryId == product.CategoryId &&
                                                 p.ProductId != product.ProductId &&
                                                 p.IsOffer &&
                                                !p.IsDeleted &&
                                                 p.IsAvaiable);

            if (noOtherProductsHaveOffer)
            {
                category.IsOffer = false;
            }


            return true;
        }

        public async Task<bool> IsProductNameTaken(string productName, int? productId)
        {
            var trimmedName = productName.Trim();

            return await _context.Products.AnyAsync(p =>
                EF.Functions.Like(p.ProductName.Trim(), trimmedName) &&
                (!productId.HasValue || p.ProductId != productId.Value));
        }

    }
}
