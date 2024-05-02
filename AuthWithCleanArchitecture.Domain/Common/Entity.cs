namespace AuthWithCleanArchitecture.Domain.Common;

public abstract class Entity<TId> : Timestamp
    where TId : IEquatable<TId>
{
    public required TId Id { get; set; }
}