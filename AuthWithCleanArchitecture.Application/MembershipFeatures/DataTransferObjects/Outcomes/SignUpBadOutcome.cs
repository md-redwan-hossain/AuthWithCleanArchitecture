namespace AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects.Outcomes;

public enum SignUpBadOutcome : byte
{
    Duplicate = 1,
    PasswordTooWeek
}