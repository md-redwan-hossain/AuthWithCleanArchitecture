using AuthWithCleanArchitecture.Domain.AppUserAggregate;

namespace AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects;

public record AppUserSignUpResponse(AppUserId Id, string FullName, string UserName, DateTime CreatedAtUtc);