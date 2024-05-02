using AuthWithCleanArchitecture.Domain.MembershipEntities;
using AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;

namespace AuthWithCleanArchitecture.Domain.Repositories;

public interface IAppUserRepository : IRepositoryBase<AppUser, AppUserId>
{
}