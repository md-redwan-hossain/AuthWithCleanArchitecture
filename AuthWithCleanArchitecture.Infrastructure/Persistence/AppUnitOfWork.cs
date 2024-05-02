using AuthWithCleanArchitecture.Application;
using AuthWithCleanArchitecture.Domain.Repositories;


namespace AuthWithCleanArchitecture.Infrastructure.Persistence;

public class AppUnitOfWork : UnitOfWork, IAppUnitOfWork
{
    public AppUnitOfWork(AppDbContext appDbContext, IAppUserRepository appUserRepository)
        : base(appDbContext)
    {
        AppUserRepository = appUserRepository;
    }

    public IAppUserRepository AppUserRepository { get; }
}