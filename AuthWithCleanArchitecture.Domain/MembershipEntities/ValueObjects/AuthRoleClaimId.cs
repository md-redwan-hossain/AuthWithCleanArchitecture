namespace AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;

public readonly record struct AuthRoleClaimId
{
    public required Guid Data { get; init; }
}