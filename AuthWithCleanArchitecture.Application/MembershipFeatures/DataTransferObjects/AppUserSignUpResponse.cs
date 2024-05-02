using AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;

namespace AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects;

public record AppUserSignUpResponse(AppUserId Id, string FullName, string UserName, DateTime CreatedAtUtc);