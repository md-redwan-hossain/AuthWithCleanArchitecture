using System.Text;
using AuthWithCleanArchitecture.Application.Common.Options;
using AuthWithCleanArchitecture.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AuthWithCleanArchitecture.Infrastructure.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection BindAndValidateOptions<TOptions>(this IServiceCollection services,
        string sectionName) where TOptions : class
    {
        services.AddOptions<TOptions>()
            .BindConfiguration(sectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        return services;
    }

    public static async Task<IServiceCollection> AddDatabaseConfigAsync(this IServiceCollection services,
        IConfiguration configuration)
    {
        var dbUrl = configuration.GetSection(AppSecretOptions.SectionName)
            .GetValue<string>(nameof(AppSecretOptions.DatabaseUrl));

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite(dbUrl);

        // await using (var dbContext = new AppDbContext(optionsBuilder.Options))
        // {
        //     var canConnect = await dbContext.Database.CanConnectAsync();
        //     if (canConnect is false) throw new Exception("Database is not functional");
        //     var migrations = await dbContext.Database.GetAppliedMigrationsAsync();
        //     if (migrations.Any() is false) await dbContext.Database.MigrateAsync();
        // }

        services.AddDbContext<AppDbContext>(
            dbContextOptions => dbContextOptions
                .UseSqlite(dbUrl)
                .UseEnumCheckConstraints()
                .LogTo(Console.WriteLine, LogLevel.Error)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
        );

        return services;
    }


    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        ArgumentNullException.ThrowIfNull(jwtOptions);

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
                };
            });
        return services;
    }
}