using AuthWithCleanArchitecture.Domain.Common;
using AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;

namespace AuthWithCleanArchitecture.Domain.MembershipEntities;

public class AppUser : Entity<AppUserId>
{
    public required string FullName { get; set; }
    public required string UserName { get; set; }
    public required string PasswordHash { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public bool IsPhoneNumberConfirmed { get; set; }
    public bool IsBanned { get; set; }
    public bool IsLockedOut { get; set; }
    public bool CanLockedOut { get; set; } = true;
    public DateTime? LockoutEndAtUtc { get; set; }
    public int AccessFailCount { get; set; }

    public bool IsVerified => IsEmailConfirmed || IsPhoneNumberConfirmed;
}