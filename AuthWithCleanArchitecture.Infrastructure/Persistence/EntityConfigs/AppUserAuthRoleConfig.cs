using AuthWithCleanArchitecture.Domain.MembershipEntities;
using AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthWithCleanArchitecture.Infrastructure.Persistence.EntityConfigs;

public class AppUserAuthRoleConfig : IEntityTypeConfiguration<AppUserAuthRole>
{
    public void Configure(EntityTypeBuilder<AppUserAuthRole> builder)
    {
        builder.ToTable(nameof(AppUserAuthRole) + 's');
        
        builder.HasKey(x => new { x.AppUserId, x.AuthRoleId });
        
        builder.Property(e => e.AppUserId)
            .HasConversion(
                convertToProviderExpression: value => value.Data,
                convertFromProviderExpression: value => new AppUserId { Data = value }
            );

        builder.Property(e => e.AuthRoleId)
            .HasConversion(
                convertToProviderExpression: value => value.Data,
                convertFromProviderExpression: value => new AuthRoleId { Data = value }
            );

        builder
            .HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(x => x.AppUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne<AuthRole>()
            .WithMany()
            .HasForeignKey(x => x.AuthRoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}