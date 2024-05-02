using AuthWithCleanArchitecture.Application;
using AuthWithCleanArchitecture.Application.AuthCryptographyFeatures;
using AuthWithCleanArchitecture.Application.Common.Providers;
using AuthWithCleanArchitecture.Domain.Repositories;
using AuthWithCleanArchitecture.Infrastructure.Common.Providers;
using AuthWithCleanArchitecture.Infrastructure.Persistence;
using AuthWithCleanArchitecture.Infrastructure.Persistence.Repositories;
using AuthWithCleanArchitecture.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AuthWithCleanArchitecture.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
    {
        services.TryAddScoped<IAppUserRepository, AppUserRepository>();
        services.TryAddScoped<IAppUnitOfWork, AppUnitOfWork>();
        services.TryAddSingleton<IAuthCryptographyService, AuthCryptographyService>();
        services.TryAddSingleton<IGuidProvider, GuidProvider>();
        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.TryAddSingleton<IJwtProvider, JwtProvider>();

        return services;
    }
}