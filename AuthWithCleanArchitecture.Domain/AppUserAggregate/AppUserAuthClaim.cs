using AuthWithCleanArchitecture.Domain.Common;

namespace AuthWithCleanArchitecture.Domain.AppUserAggregate;

public class AppUserAuthClaim : Timestamp
{
    public required AppUserId AppUserId { get; set; }
    public required string ClaimTag { get; set; }
    public required string ClaimValue { get; set; }
}