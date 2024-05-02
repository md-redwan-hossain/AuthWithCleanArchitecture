namespace AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects;

public record AppUserSignUpRequest(string FullName, string UserName, string Password,  string ConfirmPassword);