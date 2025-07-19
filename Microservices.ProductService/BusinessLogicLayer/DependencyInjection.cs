using Azure.Messaging.ServiceBus;
using BusinessLogicLayer.Mappers;
using BusinessLogicLayer.MessageBroker;
using BusinessLogicLayer.MessageBroker.Contracts;
using BusinessLogicLayer.MessageBroker.HostedServices;
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
        services.AddAutoMapper(typeof(ProductAddRequestToProductMappingProfile).Assembly);
        services.AddAutoMapper(typeof(ProductUpdateRequestToProductMappingProfile).Assembly);
        services.AddAutoMapper(typeof(ProductToProductResponseMappingProfile).Assembly);

        services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ProductUpdateRequestValidator>();

        services.AddScoped<IProductService, ProductService>();
        
        services.AddTransient<IMessagePublisher, RabbitMQPublisher>();

        services.AddSingleton(_ =>
        {
            var connectionString = configuration.GetConnectionString("AzureServiceBus");

            return new ServiceBusClient(connectionString);
        });
        services.AddSingleton<IServiceBusPublisher, ServiceBusPublisher>();
        services.AddSingleton<IServiceBusOrderPlacedConsumer, ServiceBusOrderPlacedConsumer>();

        services.AddHostedService<ServiceBusOrderPlacedHostedService>();

        return services;
    }
}