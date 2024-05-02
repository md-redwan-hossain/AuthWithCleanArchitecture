using AuthWithCleanArchitecture.Domain.Common;
using AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;

namespace AuthWithCleanArchitecture.Domain.MembershipEntities;

public class AuthRoleClaim : Entity<AuthRoleClaimId>
{
    public required AuthRoleId AuthRoleId { get; set; }
    public required string ClaimTag { get; set; }
    public required string ClaimValue { get; set; }
}