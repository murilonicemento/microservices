using FluentValidation;
using UserService.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using UserService.Core.ServicesContracts;
using UserService.Core.Validators;

namespace UserService.Core;

public static class DependencyInjection
{
    /// <summary>
    /// Extension method to add core services to the dependency injection container
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddTransient<IUserService, Services.UserService>();

        services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

        return services;
    }
}