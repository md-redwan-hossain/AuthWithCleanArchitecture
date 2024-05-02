using System.Reflection;
using AuthWithCleanArchitecture.Domain.AppUserAggregate;
using AuthWithCleanArchitecture.Domain.AuthRoleAggregate;
using Microsoft.EntityFrameworkCore;

namespace AuthWithCleanArchitecture.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<AuthRole> AuthRoles => Set<AuthRole>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>().Property(e => e.Id)
            .HasConversion(
                convertToProviderExpression: value => value.Data,
                convertFromProviderExpression: value => new AppUserId(value)
            );

        modelBuilder.Entity<AuthRole>().Property(e => e.Id)
            .HasConversion(
                convertToProviderExpression: value => value.Data,
                convertFromProviderExpression: value => new AuthRoleId(value)
            );


        modelBuilder.Entity<AppUserAuthRole>().Property(e => e.AppUserId)
            .HasConversion(
                convertToProviderExpression: value => value.Data,
                convertFromProviderExpression: value => new AppUserId(value)
            );

        modelBuilder.Entity<AppUserAuthRole>().Property(e => e.AuthRoleId)
            .HasConversion(
                convertToProviderExpression: value => value.Data,
                convertFromProviderExpression: value => new AuthRoleId(value)
            );


        modelBuilder.Entity<AppUserAuthClaim>().Property(e => e.AppUserId)
            .HasConversion(
                convertToProviderExpression: value => value.Data,
                convertFromProviderExpression: value => new AppUserId(value)
            );


        modelBuilder.Entity<AppUserAuthRole>().HasKey(x => new { x.AppUserId, x.AuthRoleId });
        modelBuilder.Entity<AppUserAuthClaim>().HasKey(x => new { x.AppUserId, x.ClaimTag });

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}