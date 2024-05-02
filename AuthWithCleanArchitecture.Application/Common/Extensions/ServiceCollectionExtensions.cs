using AuthWithCleanArchitecture.Application.Common.Utils;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AuthWithCleanArchitecture.Application.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFluentValidationRules(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<IApplicationAssemblyMarker>();
        return services;
    }
}