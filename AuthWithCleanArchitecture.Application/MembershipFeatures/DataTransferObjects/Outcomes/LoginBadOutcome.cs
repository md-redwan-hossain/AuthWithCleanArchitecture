namespace AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects.Outcomes;

public enum LoginBadOutcome : byte
{
    UserNotFound = 1,
    PasswordNotMatched,
    Banned,
    LockedOut
}