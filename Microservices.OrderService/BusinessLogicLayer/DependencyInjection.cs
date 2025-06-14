using BusinessLogicLayer.Mappers;
using BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
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

        return services;
    }
}