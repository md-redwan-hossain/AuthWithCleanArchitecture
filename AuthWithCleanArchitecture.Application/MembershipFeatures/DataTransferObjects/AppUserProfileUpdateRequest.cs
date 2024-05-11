namespace AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects;

public record AppUserProfileUpdateRequest(string FullName, string? Email, string? PhoneNumber);