using SmartCart.Application.Common;
using SmartCart.Application.Dto;
using SmartCart.Application.Dto.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<GenericResult<PaginatedResult<CategoryDto>>> GetAllPaginatedCategories(int page, int pageSize);
        Task<GenericResult<IEnumerable<CategoryDto>>> GetCategoriesWithOffers();
        Task<Result> CreateCategory(CreateCategoryDto createCategoryDto);
        Task<Result> UpdateCategory(CategoryDto categoryDto);
    }
}
