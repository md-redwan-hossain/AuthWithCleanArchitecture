using AuthWithCleanArchitecture.Domain.AppUserAggregate;
using AuthWithCleanArchitecture.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthWithCleanArchitecture.Infrastructure.Persistence.Repositories;

public class AppUserRepository : Repository<AppUser, AppUserId>, IAppUserRepository
{
    public AppUserRepository(AppDbContext context) : base(context)
    {
    }
}