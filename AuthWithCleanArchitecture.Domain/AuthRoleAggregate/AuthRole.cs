using AuthWithCleanArchitecture.Domain.Common;

namespace AuthWithCleanArchitecture.Domain.AuthRoleAggregate;

public class AuthRole : Entity<AuthRoleId>
{
    public required string Tag { get; set; }
}