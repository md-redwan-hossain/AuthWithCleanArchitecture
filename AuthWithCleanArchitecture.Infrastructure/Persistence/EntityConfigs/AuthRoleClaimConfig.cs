using AuthWithCleanArchitecture.Domain.MembershipEntities;
using AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthWithCleanArchitecture.Infrastructure.Persistence.EntityConfigs;

public class AuthRoleClaimConfig : IEntityTypeConfiguration<AuthRoleClaim>
{
    public void Configure(EntityTypeBuilder<AuthRoleClaim> builder)
    {
        builder.ToTable(nameof(AuthRoleClaim) + 's');

        builder.Property(e => e.Id)
            .HasConversion(
                convertToProviderExpression: value => value.Data,
                convertFromProviderExpression: value => new AuthRoleClaimId(value)
            );

        builder.Property(e => e.AuthRoleId)
            .HasConversion(
                convertToProviderExpression: value => value.Data,
                convertFromProviderExpression: value => new AuthRoleId(value)
            );
    }
}