using DataAccessLayer.Context;
using DataAccessLayer.Repositories;
using DataAccessLayer.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("MySQL")!;

            options.UseMySQL(connectionString);
        });
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}