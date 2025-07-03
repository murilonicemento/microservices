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
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

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
        services.AddTransient<IMessageUpdateMessageConsumer, RabbitMQProductNameUpdateConsumer>();
        services.AddTransient<IMessageDeletionConsumer, RabbitMQProductDeletionConsumer>();

        services.AddHostedService<ProductNameUpdateHostedService>();
        services.AddHostedService<ProductDeletionMessageHostedService>();

        services.AddStackExchangeRedisCache(options =>
        {
            var connectionString = configuration.GetConnectionString("Redis");
            options.Configuration = connectionString;
        });

        return services;
    }
}