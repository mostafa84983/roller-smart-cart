using AutoMapper;

using SmartCart.Application.Dto;
using SmartCart.Application.Dto.Category;

using SmartCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Helper
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<Order, OrderDto>();
        }
    }
}
