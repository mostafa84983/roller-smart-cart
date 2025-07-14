using SmartCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Domain.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<(IEnumerable<Category> Data, int TotalCount)> GetPaginatedCategoriesWithOffers(int page, int pageSize);
        Task<bool> IsCategoryNameTaken(string categoryName, int? categoryId);
    }
}
