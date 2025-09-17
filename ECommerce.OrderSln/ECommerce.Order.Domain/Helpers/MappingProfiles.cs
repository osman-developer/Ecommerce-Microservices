using AutoMapper;
using ECommerce.Order.Domain.DTOs.Core.Order;
using ECommerce.Order.Domain.DTOs.Core.Orderline;
using ECommerce.Order.Domain.Entities;

namespace ECommerce.Order.Domain.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // PurchaseOrder -> GetPurchaseOrderDTO
            CreateMap<PurchaseOrder, GetPurchaseOrderDTO>()
                .ForMember(dest => dest.OrderLines, opt => opt.MapFrom(src => src.OrderLines));

            // AddOrUpdatePurchaseOrderDTO -> PurchaseOrder
            CreateMap<AddOrUpdatePurchaseOrderDTO, PurchaseOrder>()
              .ForMember(dest => dest.OrderLines, opt => opt.MapFrom(src => src.OrderLines));


            // OrderLine -> GetOrderLineDTO
            CreateMap<OrderLine, GetOrderLineDTO>();

            // AddOrUpdateOrderLineDTO -> OrderLine
            CreateMap<AddOrUpdateOrderLineDTO, OrderLine>()
                .ForMember(dest => dest.PurchaseOrder, opt => opt.Ignore());
        }
    }
}
