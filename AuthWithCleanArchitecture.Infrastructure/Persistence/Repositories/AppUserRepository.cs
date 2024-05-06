using AuthWithCleanArchitecture.Domain.MembershipEntities;
using AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;
using AuthWithCleanArchitecture.Domain.Repositories;

namespace AuthWithCleanArchitecture.Infrastructure.Persistence.Repositories;

public class AppUserRepository : Repository<AppUser, AppUserId>, IAppUserRepository
{
    public AppUserRepository(AppDbContext context) : base(context)
    {
    }
}