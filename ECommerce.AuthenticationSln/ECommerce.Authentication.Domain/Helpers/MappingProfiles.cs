using AutoMapper;
using ECommerce.Authentication.Domain.DTOs;
using ECommerce.Authentication.Domain.Entities;

namespace ECommerce.Authentication.Domain.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AppUser, AuthenticatedUserDTO>()
              .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
              .ForMember(dest => dest.Token, opt => opt.Ignore())
              .ForMember(dest => dest.Roles, opt => opt.Ignore());

            CreateMap<RegisterDTO, AppUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
