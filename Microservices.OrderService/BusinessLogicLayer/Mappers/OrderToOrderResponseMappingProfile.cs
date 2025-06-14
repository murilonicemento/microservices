﻿using AutoMapper;
using BusinessLogicLayer.DTO;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Mappers;

public class OrderToOrderResponseMappingProfile : Profile
{
    public OrderToOrderResponseMappingProfile()
    {
        CreateMap<Order, OrderResponse>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
            .ForMember(dest => dest.TotalBill, opt => opt.MapFrom(src => src.TotalBill));
    }
}