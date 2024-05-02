using AuthWithCleanArchitecture.Domain.Common;
using AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;

namespace AuthWithCleanArchitecture.Domain.MembershipEntities;

public class AppUserAuthClaim : Entity<AppUserAuthClaimId>
{
    public required AppUserId AppUserId { get; set; }
    public required string ClaimTag { get; set; }
    public required string ClaimValue { get; set; }
}