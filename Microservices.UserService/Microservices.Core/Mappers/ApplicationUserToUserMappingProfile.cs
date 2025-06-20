﻿using AutoMapper;
using Microservices.Core.DTO;
using Microservices.Core.Entities;

namespace Microservices.Core.Mappers;

public class ApplicationUserToUserMappingProfile : Profile
{
    public ApplicationUserToUserMappingProfile()
    {
        CreateMap<ApplicationUser, User>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.PersonName))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender));
    }
}