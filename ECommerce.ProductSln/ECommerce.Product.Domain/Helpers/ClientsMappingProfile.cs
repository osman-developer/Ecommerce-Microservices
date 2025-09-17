using AutoMapper;
using ECommerce.Product.Domain.DTOs.Clients;
using ECommerce.Product.Domain.Entities;

namespace ECommerce.Product.Domain.Helpers
{
    public class ClientsMappingProfile : Profile
    {
        public ClientsMappingProfile()
        {
            CreateMap<ProductItem, ProductOrderDTO>();
        }
    }
}
