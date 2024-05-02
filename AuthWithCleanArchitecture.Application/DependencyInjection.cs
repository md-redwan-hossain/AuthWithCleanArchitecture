using AuthWithCleanArchitecture.Application.AuthCryptographyFeatures;
using AuthWithCleanArchitecture.Application.Common.Extensions;
using AuthWithCleanArchitecture.Application.Common.Providers;
using AuthWithCleanArchitecture.Application.MembershipFeatures;
using AuthWithCleanArchitecture.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AuthWithCleanArchitecture.Application;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        services.TryAddScoped<IMembershipService, MembershipService>();
        services.AddFluentValidationRules();
        return services;
    }
}