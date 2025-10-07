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

            CreateMap<AppUser, AuthenticatedUserDTO>()
               .ConstructUsing(user => new AuthenticatedUserDTO(
                   user.Id,
                   user.Email!,
                   user.FirstName!,
                   user.LastName!,
                   string.Empty,      // Token will be set later
                   user.CustomerId,   // Optional, can be null
                   new List<string>() // Roles will be set later
               ));
        }
    }
}
