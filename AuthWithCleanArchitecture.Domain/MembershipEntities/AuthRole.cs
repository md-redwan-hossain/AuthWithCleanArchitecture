using AuthWithCleanArchitecture.Domain.Common;
using AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;

namespace AuthWithCleanArchitecture.Domain.MembershipEntities;

public class AuthRole : Entity<AuthRoleId>
{
    public required string Tag { get; set; }
}