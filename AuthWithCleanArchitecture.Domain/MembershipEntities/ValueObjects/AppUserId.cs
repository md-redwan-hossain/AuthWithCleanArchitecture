namespace AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;

public readonly record struct AppUserId
{
    public required Guid Data { get; init; }
}