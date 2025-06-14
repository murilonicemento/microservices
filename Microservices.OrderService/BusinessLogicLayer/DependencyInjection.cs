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
        
        return services;
    }
}