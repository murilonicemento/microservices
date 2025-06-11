using FluentValidation;
using Microservices.Core.Services;
using Microservices.Core.ServicesContracts;
using Microservices.Core.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Core;

public static class DependencyInjection
{
    /// <summary>
    /// Extension method to add core services to the dependency injection container
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();

        services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

        return services;
    }
}