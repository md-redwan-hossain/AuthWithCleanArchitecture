namespace AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;

public readonly record struct AuthRoleId
{
    public required Guid Data { get; init; }
}