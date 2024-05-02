using AuthWithCleanArchitecture.Domain.AuthRoleAggregate;
using AuthWithCleanArchitecture.Domain.Common;

namespace AuthWithCleanArchitecture.Domain.AppUserAggregate;

public class AppUserAuthRole : Timestamp
{
    public required AppUserId AppUserId { get; set; }
    public required AuthRoleId AuthRoleId { get; set; }
}