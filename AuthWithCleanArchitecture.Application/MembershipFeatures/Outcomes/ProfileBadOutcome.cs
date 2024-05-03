namespace AuthWithCleanArchitecture.Application.MembershipFeatures.Outcomes;

public enum ProfileBadOutcome: byte
{
    UserNotFound = 1,
    Banned,
    LockedOut
}