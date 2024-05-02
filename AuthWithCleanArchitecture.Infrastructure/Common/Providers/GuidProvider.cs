using AuthWithCleanArchitecture.Application.Common.Providers;

namespace AuthWithCleanArchitecture.Infrastructure.Common.Providers;

public class GuidProvider : IGuidProvider
{
    public Guid SortableGuid() => Ulid.NewUlid().ToGuid();

    public Guid RandomGuid() => Guid.NewGuid();
}