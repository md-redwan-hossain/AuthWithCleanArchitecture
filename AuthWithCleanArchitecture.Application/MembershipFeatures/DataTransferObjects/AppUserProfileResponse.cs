using AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;

namespace AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects;

public record AppUserProfileResponse(
    AppUserId Id,
    string FullName,
    string UserName,
    string? Email,
    string? PhoneNumber,
    bool IsEmailConfirmed,
    bool IsPhoneNumberConfirmed,
    DateTime CreatedAtUtc);