using AutoMapper;
using BusinessLogicLayer.DTO;

namespace BusinessLogicLayer.Mappers;

public class ProductToOrderItemResponseMappingProfile : Profile
{
    public ProductToOrderItemResponseMappingProfile()
    {
        CreateMap<Product, OrderItemResponse>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
    }
}