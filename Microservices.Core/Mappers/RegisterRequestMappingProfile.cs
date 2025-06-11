using AutoMapper;
using Microservices.Core.DTO;
using Microservices.Core.Entities;

namespace Microservices.Core.Mappers;

public class RegisterRequestMappingProfile : Profile
{
    public RegisterRequestMappingProfile()
    {
        CreateMap<RegisterRequest, ApplicationUser>()
            .ForMember(dest => dest.UserId, src => src.Ignore())
            .ForMember(dest => dest.Email, src => src.MapFrom(opt => opt.Email))
            .ForMember(dest => dest.Gender, src => src.MapFrom(opt => opt.Gender))
            .ForMember(dest => dest.PersonName, src => src.MapFrom(opt => opt.PersonName))
            .ForMember(dest => dest.Password, src => src.MapFrom(opt => opt.Password));
    }
}