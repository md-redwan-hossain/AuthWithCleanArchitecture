using AuthWithCleanArchitecture.Domain.Common;
using AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;

namespace AuthWithCleanArchitecture.Domain.MembershipEntities;

public class AppUser : Entity<AppUserId>
{
    private bool _isEmailConfirmed;
    private bool _isPhoneNumberConfirmed;
    private bool _isLockedOut;
    private DateTime? _lockoutEndAtUtc;
    public required string FullName { get; set; }
    public required string UserName { get; set; }
    public required string PasswordHash { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    public bool IsEmailConfirmed
    {
        get => _isEmailConfirmed;
        set
        {
            if (value && Email is not null) _isEmailConfirmed = value;
        }
    }

    public bool IsPhoneNumberConfirmed
    {
        get => _isPhoneNumberConfirmed;
        set
        {
            if (value && PhoneNumber is not null) _isPhoneNumberConfirmed = value;
        }
    }

    public bool IsBanned { get; set; }
    public bool CanLockedOut { get; set; } = true;

    public bool IsLockedOut
    {
        get => _isLockedOut;
        set
        {
            if (value && CanLockedOut) _isLockedOut = value;
        }
    }

    public DateTime? LockoutEndAtUtc
    {
        get => _lockoutEndAtUtc;
        set
        {
            if (value is not null && CanLockedOut) _lockoutEndAtUtc = value;
        }
    }

    public int AccessFailCount { get; set; }
    public bool IsVerified => IsEmailConfirmed || IsPhoneNumberConfirmed;
}