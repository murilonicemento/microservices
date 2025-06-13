using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStringTemplate = configuration.GetConnectionString("MongoDB")!;
        var connectionString = connectionStringTemplate
            .Replace("$MONGODB_HOST", Environment.GetEnvironmentVariable("MONGODB_HOST"))
            .Replace("$MONGODB_PORT", Environment.GetEnvironmentVariable("MONGODB_PORT"));

        services.AddSingleton<IMongoClient>(new MongoClient(connectionString));
        services.AddScoped<IMongoDatabase>(provider =>
        {
            var client = provider.GetRequiredService<IMongoClient>();

            return client.GetDatabase("OrderDatabase");
        });

        return services;
    }
}