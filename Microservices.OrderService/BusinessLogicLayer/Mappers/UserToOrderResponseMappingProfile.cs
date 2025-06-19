using AutoMapper;
using BusinessLogicLayer.DTO;

namespace BusinessLogicLayer.Mappers;

public class UserToOrderResponseMappingProfile : Profile
{
    public UserToOrderResponseMappingProfile()
    {
        CreateMap<User, OrderResponse>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.PersonName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
    }
}