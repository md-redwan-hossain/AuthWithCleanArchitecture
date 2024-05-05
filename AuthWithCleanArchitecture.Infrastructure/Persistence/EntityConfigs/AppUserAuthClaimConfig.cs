using AuthWithCleanArchitecture.Domain.MembershipEntities;
using AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthWithCleanArchitecture.Infrastructure.Persistence.EntityConfigs;

public class AppUserAuthClaimConfig : IEntityTypeConfiguration<AppUserAuthClaim>
{
    public void Configure(EntityTypeBuilder<AppUserAuthClaim> builder)
    {
        builder.ToTable(nameof(AppUserAuthClaim) + 's');

        builder.Property(e => e.Id)
            .HasConversion(
                convertToProviderExpression: value => value.Data,
                convertFromProviderExpression: value => new AppUserAuthClaimId { Data = value }
            );

        builder.Property(e => e.AppUserId)
            .HasConversion(
                convertToProviderExpression: value => value.Data,
                convertFromProviderExpression: value => new AppUserId { Data = value }
            );

        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(x => x.AppUserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}