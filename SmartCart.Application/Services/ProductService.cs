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
           var (productsData , totalCount) = await  _unitOfWork.Product.GetPaginatedProductsInCategory(categoryId, page, pageSize);

            if (productsData == null)
            {
                return GenericResult<PaginatedResult<ProductDto>>.Failure("Could not retrieve products in this category");
            }

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
            var (productsData, totalCount) = await _unitOfWork.Product.GetPaginatedProductsWithOfferInCategory(categoryId, page, pageSize);

            if (productsData == null)
            {
                return GenericResult<PaginatedResult<ProductDto>>.Failure("Could not retrieve products with offers in this category");
            }

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
            if (productsData == null)
            {
                return GenericResult<PaginatedResult<ProductDto>>.Failure("Could not retrieve products in this order");
            }

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

        public async Task<Result> AddOfferToProduct(int productId, decimal offerPercentage)
        {
            if (offerPercentage < 0 || offerPercentage > 100)
                return Result.Failure("Offer percentage must be between 0 and 100");

            var result = await _unitOfWork.Product.AddOfferToProduct(productId, offerPercentage);
            if (!result)
                return Result.Failure("Failed to add offer: Product not found, deleted or unavailable");

            _unitOfWork.Save();
            return Result.Success();
        }

        public async Task<Result> RemoveOfferFromProduct(int productId)
        {
            var result = await _unitOfWork.Product.RemoveOfferFromProduct(productId);
            if (!result)
                return Result.Failure("Failed to remove offer: product not found or doesn't have an active offer");

            _unitOfWork.Save();
            return Result.Success();
        }

        public async Task<Result> CreateProduct(CreateProductDto product)
        {
            if (product == null)
                return Result.Failure("Product data must be provided");

            var existingProductCode = await _unitOfWork.Product.GetProductByCode(product.ProductCode);

            if(existingProductCode != null)
                return Result.Failure("Product code already exists");

            var newProduct = _mapper.Map<Product>(product);

            newProduct.IsAvaiable = true;
            newProduct.IsDeleted = false;
            newProduct.IsOffer = false;
            newProduct.OfferPercentage = 0m;

            await _unitOfWork.Product.Add(newProduct);
            _unitOfWork.Save();

            return Result.Success();
        }

        public async Task<Result> UpdateProduct(UpdateProductDto productDto)
        {
            if (productDto == null)
            {
                return Result.Failure("Product data must be provided");
            }

            if (productDto.ProductId <= 0)
            {
                return Result.Failure("Invalid ID");
            }

            var product = await _unitOfWork.Product.GetById(productDto.ProductId);
            if (product == null)
            {
                return Result.Failure("Product not found");
            }

            if (!string.IsNullOrWhiteSpace(productDto.ProductName))
                product.ProductName = productDto.ProductName;

            if (productDto.ProductWeight.HasValue)
            {
                if (productDto.ProductWeight <= 0)
                    return Result.Failure("Product weight must be greater than 0");

                product.ProductWeight = productDto.ProductWeight.Value;
            }

            if (productDto.ProductPrice.HasValue)
            {
                if (productDto.ProductPrice <= 0)
                    return Result.Failure("Product price must be greater than 0");

                product.ProductPrice = productDto.ProductPrice.Value;
            }

            if (productDto.Quantity.HasValue)
            {
                if (productDto.Quantity <= 0)
                    return Result.Failure("Product quantity must be greater than 0");

                product.Quantity = productDto.Quantity.Value;
            }



            if (!string.IsNullOrWhiteSpace(productDto.ProductImage))
                product.ProductImage = productDto.ProductImage;

            if (!string.IsNullOrWhiteSpace(productDto.ProductDescription))
                product.ProductDescription = productDto.ProductDescription;

            if (productDto.IsAvaiable.HasValue)
                product.IsAvaiable = productDto.IsAvaiable.Value;

            _unitOfWork.Product.Update(product);
            _unitOfWork.Save();

            return Result.Success();
        }
    }
}
