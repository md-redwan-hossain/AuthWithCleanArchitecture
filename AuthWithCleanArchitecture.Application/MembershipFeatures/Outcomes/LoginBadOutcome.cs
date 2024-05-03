namespace AuthWithCleanArchitecture.Application.MembershipFeatures.Outcomes;

public enum LoginBadOutcome : byte
{
    UserNotFound = 1,
    PasswordNotMatched,
    Banned,
    LockedOut
}