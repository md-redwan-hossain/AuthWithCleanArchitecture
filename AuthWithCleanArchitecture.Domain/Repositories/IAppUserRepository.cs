using AuthWithCleanArchitecture.Domain.AppUserAggregate;

namespace AuthWithCleanArchitecture.Domain.Repositories;

public interface IAppUserRepository : IRepositoryBase<AppUser, AppUserId>
{
}