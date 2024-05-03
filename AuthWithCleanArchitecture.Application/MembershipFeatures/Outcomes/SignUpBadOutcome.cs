namespace AuthWithCleanArchitecture.Application.MembershipFeatures.Outcomes;

public enum SignUpBadOutcome : byte
{
    Duplicate = 1,
    PasswordTooWeek
}