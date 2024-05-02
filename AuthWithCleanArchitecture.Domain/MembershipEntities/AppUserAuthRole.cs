using AuthWithCleanArchitecture.Domain.Common;
using AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;

namespace AuthWithCleanArchitecture.Domain.MembershipEntities;

public class AppUserAuthRole : Timestamp
{
    public required AppUserId AppUserId { get; set; }
    public required AuthRoleId AuthRoleId { get; set; }
}