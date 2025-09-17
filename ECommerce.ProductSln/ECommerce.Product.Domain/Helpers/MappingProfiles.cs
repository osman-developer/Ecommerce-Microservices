using AutoMapper;
using ECommerce.Product.Domain.DTOs.Core.Product;
using ECommerce.Product.Domain.Entities;

namespace ECommerce.Product.Domain.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ProductItem, GetProductDTO>();
            CreateMap<AddOrUpdateProductDto, ProductItem>();
        }
    }
}