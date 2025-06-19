using AutoMapper;
using SmartCart.Application.Common;
using SmartCart.Application.Dto;
using SmartCart.Application.Dto.Category;
using SmartCart.Application.Interfaces;
using SmartCart.Domain.Interfaces;
using SmartCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper) 
        { 
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

 
        public async Task<GenericResult<PaginatedResult<CategoryDto>>> GetAllPaginatedCategories(int page, int pageSize)
        {
            var (categoriesData, totalCount) = await _unitOfWork.Category.GetAllPaginated(page, pageSize);
            if (categoriesData == null || !categoriesData.Any())
            {
                return GenericResult<PaginatedResult<CategoryDto>>.Failure("Categories are not found");
            }

            var categoryDtos = _mapper.Map<List<CategoryDto>>(categoriesData);
            var paginatedResult = new PaginatedResult<CategoryDto>
            {
                Data = categoryDtos,
                TotalCount = totalCount
            };

            return GenericResult<PaginatedResult<CategoryDto>>.Success(paginatedResult);

        }

        public async Task<GenericResult<IEnumerable<CategoryDto>>> GetCategoriesWithOffers()
        {
            var categories = await _unitOfWork.Category.GetCategoriesWithOffers();
            if (categories == null || !categories.Any()) 
            {
                return GenericResult<IEnumerable<CategoryDto>>.Failure("Categories with offers are not found");
            }
            var categoryDto = _mapper.Map<List<CategoryDto>>(categories);
            return GenericResult<IEnumerable<CategoryDto>>.Success(categoryDto);

        }

        public async Task<Result> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            if (createCategoryDto == null)
            {
                return Result.Failure("Category data must be provided");
            }

            var category = _mapper.Map<Category>(createCategoryDto);
            await _unitOfWork.Category.Add(category);
            _unitOfWork.Save();

            return Result.Success();
        }


        public async Task<Result> UpdateCategory(CategoryDto categoryDto)
        {
            if(categoryDto == null)
            {
                return Result.Failure("Category data must be provided");
            }

            if(categoryDto.CategoryId <= 0)
            {
                return Result.Failure("Invalid ID");
            }

            var category = await _unitOfWork.Category.GetById(categoryDto.CategoryId);
            if (category == null)
            {
                return Result.Failure("Category not found");
            }

            _mapper.Map(categoryDto, category);
            _unitOfWork.Category.Update(category);
            _unitOfWork.Save();

            return Result.Success();
        }
    }
}
