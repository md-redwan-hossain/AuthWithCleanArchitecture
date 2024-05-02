namespace AuthWithCleanArchitecture.Domain.Utils;

public record PagedData<TPayload>(ICollection<TPayload> Payload, int TotalDataCount);