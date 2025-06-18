using DataAccessLayer.Repositories;
using DataAccessLayer.RepositoriesContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDB")!;

        services.AddSingleton<IMongoClient>(new MongoClient(connectionString));
        services.AddScoped<IMongoDatabase>(provider =>
        {
            var client = provider.GetRequiredService<IMongoClient>();

            return client.GetDatabase(Environment.GetEnvironmentVariable("MONGODB_DATABASE"));
        });
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}