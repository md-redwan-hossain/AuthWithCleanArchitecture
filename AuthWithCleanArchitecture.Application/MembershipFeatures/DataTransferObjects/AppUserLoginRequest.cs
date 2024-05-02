namespace AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects;

public record AppUserLoginRequest(string UserName, string Password);