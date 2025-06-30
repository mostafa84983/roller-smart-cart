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
    public class CategoryRepository : GenericRepository<Category> , ICategoryRepository
    {
        public CategoryRepository(DataContext Context) : base(Context) 
        {

        }

        public async Task<(IEnumerable<Category> Data, int TotalCount)> GetPaginatedCategoriesWithOffers(int page, int pageSize)
        {
            var query = _context.Categories.Where(c => c.IsOffer)
                                   .AsNoTracking();

            var totalCount = await query.CountAsync();

            var paginatedData = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (paginatedData, totalCount);
        }


        public async Task<bool> IsCategoryNameTaken(string categoryName, int? categoryId)
        {
            var trimmedName = categoryName.Trim();

            return await _context.Categories.AnyAsync(c =>
                EF.Functions.Like(c.CategoryName.Trim(), trimmedName) &&
                (!categoryId.HasValue || c.CategoryId != categoryId.Value));
        }

    }
}
