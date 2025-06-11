using Microservices.Core.RepositoriesContracts;
using Microservices.Infrastructure.DbContext;
using Microservices.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Extension method to add infrastructure services to the dependency injection container
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<DapperDbContext>();

        return services;
    }
}