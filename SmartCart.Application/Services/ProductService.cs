using AutoMapper;
using SmartCart.Application.Common;
using SmartCart.Application.Dto;
using SmartCart.Application.Dto.Category;
using SmartCart.Application.Dto.Product;
using SmartCart.Application.Interfaces;
using SmartCart.Domain.Enums;
using SmartCart.Domain.Interfaces;
using SmartCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GenericResult<PaginatedResult<ProductDto>>> GetPaginatedProductsInCategory(int categoryId, int page, int pageSize)
        {
            var category = await _unitOfWork.Category.GetById(categoryId);
            if (category == null)
            {
                return GenericResult<PaginatedResult<ProductDto>>.Failure("Category not found");
            }

            var (productsData , totalCount) = await  _unitOfWork.Product.GetPaginatedProductsInCategory(categoryId, page, pageSize);

            var productDtos = _mapper.Map<List<ProductDto>>(productsData);

            var paginatedResult = new PaginatedResult<ProductDto>
            {
                Data = productDtos,
                TotalCount = totalCount
            };

            return GenericResult<PaginatedResult<ProductDto>>.Success(paginatedResult);
        }


        public async Task<GenericResult<PaginatedResult<ProductDto>>> GetPaginatedProductsWithOfferInCategory(int categoryId, int page, int pageSize)
        {
            var category = await _unitOfWork.Category.GetById(categoryId);
            if (category == null)
            {
                return GenericResult<PaginatedResult<ProductDto>>.Failure("Category not found");
            }

            var (productsData, totalCount) = await _unitOfWork.Product.GetPaginatedProductsWithOfferInCategory(categoryId, page, pageSize);

            var productDtos = _mapper.Map<List<ProductDto>>(productsData);

            var paginatedResult = new PaginatedResult<ProductDto>
            {
                Data = productDtos,
                TotalCount = totalCount
            };

            return GenericResult<PaginatedResult<ProductDto>>.Success(paginatedResult);
        }

        public async Task<GenericResult<PaginatedResult<ProductDto>>> GetPaginatedProductsOfOrder(int orderId, int page, int pageSize, int userClaims, RoleEnum role)
        {
            var order = await _unitOfWork.Order.GetById(orderId);
            if(order == null)
            {
                return GenericResult<PaginatedResult<ProductDto>>.Failure("Order not found");
            }

            if(order.UserId != userClaims && role != RoleEnum.Admin)
            {
                return GenericResult<PaginatedResult<ProductDto>>.Failure("You are not authorized to view products of this order");
            }

            var (productsData, totalCount) = await _unitOfWork.Product.GetPaginatedProductsOfOrder(orderId, page, pageSize);

            var productDtos = _mapper.Map<List<ProductDto>>(productsData);

            var paginatedResult = new PaginatedResult<ProductDto>
            {
                Data = productDtos,
                TotalCount = totalCount
            };

            return GenericResult<PaginatedResult<ProductDto>>.Success(paginatedResult);
        }

        public async Task<GenericResult<ProductDto>> GetProductByCode(int productCode)
        {
            var product = await _unitOfWork.Product.GetProductByCode(productCode);

            if (product == null) 
            {
                return GenericResult<ProductDto>.Failure("Product not found by this code");
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return GenericResult<ProductDto>.Success(productDto);
        }

        public async Task<GenericResult<ProductDto>> GetProductById(int productId)
        {
            var product = await _unitOfWork.Product.GetById(productId);

            if (product == null)
            {
                return GenericResult<ProductDto>.Failure("Product not found");
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return GenericResult<ProductDto>.Success(productDto);
        }

        public async Task<Result> SoftDeleteProduct(int productId)
        {
            var deletedProduct = await _unitOfWork.Product.SoftDeleteProduct(productId);
            if(!deletedProduct)
            {
                return Result.Failure("Product not found or already deleted");
            }

             _unitOfWork.Save();
            return Result.Success();
        }

        public async Task<Result> RestoreProduct(int productId)
        {
            var restoredProduct = await _unitOfWork.Product.RestoreProduct(productId);
            if (!restoredProduct)
            {
                return Result.Failure("Product not found or not deleted");
            }

            _unitOfWork.Save();
            return Result.Success();
        }

        public async Task<Result> AddOfferToProduct(AddOfferDto productDto)
        {

            var result = await _unitOfWork.Product.AddOfferToProduct(productDto.ProductId, productDto.OfferPercentage);
            if (!result)
                return Result.Failure("Failed to add offer to the product");

            _unitOfWork.Save();
            return Result.Success();
        }

        public async Task<Result> RemoveOfferFromProduct(RemoveOfferDto productDto)
        {
            var result = await _unitOfWork.Product.RemoveOfferFromProduct(productDto.ProductId);
            if (!result)
                return Result.Failure("Failed to remove offer from the product");

            _unitOfWork.Save();
            return Result.Success();
        }

        public async Task<Result> CreateProduct(CreateProductDto product)
        {
            if (product.ProductCode <= 0)
                return Result.Failure("Product code must be greater than 0");

            var existingProduct = await _unitOfWork.Product.GetProductByCode(product.ProductCode);
            if(existingProduct != null)
                return Result.Failure("Product code already exists");

            var isNameTaken = await _unitOfWork.Product.IsProductNameTaken(product.ProductName, null);
            if (isNameTaken)
            {
                return Result.Failure("Product name already exists");
            }

            var category = await _unitOfWork.Category.GetById(product.CategoryId);
            if (category == null)
                return Result.Failure("Category does not exist");

            var newProduct = _mapper.Map<Product>(product);

            await _unitOfWork.Product.Add(newProduct);
            _unitOfWork.Save();

            return Result.Success();
        }

        public async Task<Result> UpdateProduct(UpdateProductDto productDto)
        {
            if (productDto.ProductId <= 0)
            {
                return Result.Failure("Invalid ID");
            }

            var product = await _unitOfWork.Product.GetById(productDto.ProductId);
            if (product == null || product.IsDeleted)
            {
                return Result.Failure("Product not found");
            }

            bool hasUpdated = false;

            if (!string.IsNullOrWhiteSpace(productDto.ProductName) && !string.Equals(product.ProductName?.Trim(), productDto.ProductName.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                var isNameTaken = await _unitOfWork.Product.IsProductNameTaken(productDto.ProductName, productDto.ProductId);
                if (isNameTaken)
                {
                    return Result.Failure("Another product with the same name already exists");
                }
                product.ProductName = productDto.ProductName;
                hasUpdated = true;
            }

            if (productDto.ProductCode.HasValue)
            {
                if (productDto.ProductCode <= 0)
                    return Result.Failure("Product code must be greater than 0");

                if (product.ProductCode != productDto.ProductCode.Value)
                {
                    var existingProduct = await _unitOfWork.Product.GetProductByCode(productDto.ProductCode.Value);
                    if (existingProduct != null && existingProduct.ProductId != productDto.ProductId)
                    {
                        return Result.Failure("Another product with the same code already exists");
                    }

                    product.ProductCode = productDto.ProductCode.Value;
                    hasUpdated = true;
                }
            }

                if (productDto.ProductWeight.HasValue)
            {
                if (productDto.ProductWeight <= 0)
                    return Result.Failure("Product weight must be greater than 0");

                if (product.ProductWeight != productDto.ProductWeight.Value)
                {
                    product.ProductWeight = productDto.ProductWeight.Value;
                    hasUpdated = true;
                }
            }

            if (productDto.ProductPrice.HasValue)
            {
                if (productDto.ProductPrice <= 0)
                    return Result.Failure("Product price must be greater than 0");

                if (product.ProductPrice != productDto.ProductPrice.Value)
                {
                    product.ProductPrice = productDto.ProductPrice.Value;
                    hasUpdated = true;
                }
            }

            if (productDto.Quantity.HasValue)
            {
                if (productDto.Quantity <= 0)
                    return Result.Failure("Product quantity must be greater than 0");

                if (product.Quantity != productDto.Quantity.Value)
                {
                    product.Quantity = productDto.Quantity.Value;
                    hasUpdated = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(productDto.ProductImage) && !string.Equals(product.ProductImage?.Trim(), productDto.ProductImage.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                product.ProductImage = productDto.ProductImage;
                hasUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(productDto.ProductDescription) && !string.Equals(product.ProductDescription?.Trim(), productDto.ProductDescription.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                product.ProductDescription = productDto.ProductDescription;
                hasUpdated = true;
            }

            if (productDto.IsAvaiable.HasValue && product.IsAvaiable != productDto.IsAvaiable.Value)
            {
                product.IsAvaiable = productDto.IsAvaiable.Value;
                hasUpdated = true;
            }

            if (!hasUpdated)
            {
                return Result.Failure("No valid fields to update");
            }

            _unitOfWork.Product.Update(product);
            _unitOfWork.Save();

            return Result.Success();
        }
    }
}
