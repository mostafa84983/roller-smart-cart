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

        public async Task<IEnumerable<Category>> GetCategoriesWithOffers()
        {
            return await _context.Categories.Where(c => c.IsOffer)
                            .AsNoTracking()
                            .ToListAsync();
        }
    }
}
