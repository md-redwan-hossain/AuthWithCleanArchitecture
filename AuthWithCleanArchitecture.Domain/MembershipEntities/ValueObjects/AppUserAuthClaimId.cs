namespace AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;

public readonly record struct AppUserAuthClaimId
{
    public required Guid Data { get; init; }
}