using AutoMapper;
using SmartCart.Application.Common;
using SmartCart.Application.Dto;
using SmartCart.Application.Dto.Category;
using SmartCart.Application.Dto.Product;
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
        private readonly IFileService _fileService;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService) 
        { 
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

 
        public async Task<GenericResult<PaginatedResult<CategoryDto>>> GetAllPaginatedCategories(int page, int pageSize)
        {
            var (categoriesData, totalCount) = await _unitOfWork.Category.GetAllPaginated(page, pageSize);

            var categoryDtos = _mapper.Map<List<CategoryDto>>(categoriesData);
            var paginatedResult = new PaginatedResult<CategoryDto>
            {
                Data = categoryDtos,
                TotalCount = totalCount
            };

            return GenericResult<PaginatedResult<CategoryDto>>.Success(paginatedResult);
        }

        public async Task<GenericResult<PaginatedResult<CategoryDto>>> GetPaginatedCategoriesWithOffers(int page, int pageSize)
        {
            var (categoriesData, totalCount) = await _unitOfWork.Category.GetPaginatedCategoriesWithOffers(page, pageSize);

            var categoryDtos = _mapper.Map<List<CategoryDto>>(categoriesData);
            var paginatedResult = new PaginatedResult<CategoryDto>
            {
                Data = categoryDtos,
                TotalCount = totalCount
            }; 
            
            return GenericResult<PaginatedResult<CategoryDto>>.Success(paginatedResult);
        }

        public async Task<Result> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            createCategoryDto.CategoryName = createCategoryDto.CategoryName?.Trim();

            if (string.IsNullOrWhiteSpace(createCategoryDto.CategoryName))
            {
                return Result.Failure("Category name cannot be empty or whitespace");
            }

            var isNameTaken = await _unitOfWork.Category.IsCategoryNameTaken(createCategoryDto.CategoryName, null);
            if (isNameTaken)
            {
                return Result.Failure("Category name already exists");
            }

            var uploadResult = await _fileService.SaveImage(createCategoryDto.CategoryImage);
            if (!uploadResult.IsSuccess)
                return Result.Failure(uploadResult.ErrorMessage);

            var category = _mapper.Map<Category>(createCategoryDto);
            category.CategoryImage = uploadResult.Value;

            await _unitOfWork.Category.Add(category);
            _unitOfWork.Save();

            return Result.Success();
        }


        public async Task<Result> UpdateCategory(UpdateCategoryDto categoryDto)
        {
            if(categoryDto.CategoryId <= 0)
            {
                return Result.Failure("Invalid ID");
            }

            var category = await _unitOfWork.Category.GetById(categoryDto.CategoryId);
            if (category == null)
            {
                return Result.Failure("Category not found");
            }

            bool hasUpdated = false;

            if (!string.IsNullOrWhiteSpace(categoryDto.CategoryName) && !string.Equals(category.CategoryName?.Trim(), categoryDto.CategoryName.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                var isNameTaken = await _unitOfWork.Category.IsCategoryNameTaken(categoryDto.CategoryName, categoryDto.CategoryId);
                if (isNameTaken)
                {
                    return Result.Failure("Another category with the same name already exists");
                }
                category.CategoryName = categoryDto.CategoryName;
                hasUpdated = true;
            }

            if (categoryDto.CategoryImage != null && categoryDto.CategoryImage.Length > 0)
            {
                // Delete old image
                var deleteResult = await _fileService.DeleteImage(category.CategoryImage);
                if (!deleteResult.IsSuccess)
                    return Result.Failure(deleteResult.ErrorMessage);

                // Save new image
                var uploadResult = await _fileService.SaveImage(categoryDto.CategoryImage);
                if (!uploadResult.IsSuccess)
                    return Result.Failure(uploadResult.ErrorMessage);

                category.CategoryImage = uploadResult.Value;
                hasUpdated = true;
            }


            if (!hasUpdated)
            {
                return Result.Failure("No valid fields to update");
            }

            _unitOfWork.Category.Update(category);
            _unitOfWork.Save();

            return Result.Success();
        }
    }
}
