﻿using BusinessLogicLayer.Mappers;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.ServicesContracts;
using BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<OrderAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<OrderUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<OrderItemAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<OrderItemUpdateRequestValidator>();

        services.AddAutoMapper(typeof(OrderAddRequestToOrderMappingProfile).Assembly);
        services.AddAutoMapper(typeof(OrderItemAddRequestToOrderItemMappingProfile).Assembly);
        services.AddAutoMapper(typeof(OrderItemToOrderItemResponseMappingProfile).Assembly);
        services.AddAutoMapper(typeof(OrderItemUpdateRequestToOrderItemMappingProfile).Assembly);
        services.AddAutoMapper(typeof(OrderToOrderResponseMappingProfile).Assembly);
        services.AddAutoMapper(typeof(OrderUpdateRequestToOrderMappingProfile).Assembly);

        services.AddScoped<IOrderService, OrderService>();
        services.AddStackExchangeRedisCache(options =>
        {
            var connectionString = configuration.GetConnectionString("Redis");
            options.Configuration = connectionString;
        });

        return services;
    }
}