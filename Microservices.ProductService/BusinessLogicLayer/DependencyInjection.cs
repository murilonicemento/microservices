using BusinessLogicLayer.Mappers;
using BusinessLogicLayer.MessageBroker;
using BusinessLogicLayer.MessageBroker.Contracts;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.ServicesContracts;
using BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ProductAddRequestToProductMappingProfile).Assembly);
        services.AddAutoMapper(typeof(ProductUpdateRequestToProductMappingProfile).Assembly);
        services.AddAutoMapper(typeof(ProductToProductResponseMappingProfile).Assembly);

        services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ProductUpdateRequestValidator>();
        
        services.AddScoped<IProductService, ProductService>();
        services.AddTransient<IMessagePublisher, RabbitMQPublisher>();

        return services;
    }
}