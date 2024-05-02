using AuthWithCleanArchitecture.Domain.Repositories;
using AuthWithCleanArchitecture.Domain.Utils;

namespace AuthWithCleanArchitecture.Application;

public interface IAppUnitOfWork : IUnitOfWork
{
    public IAppUserRepository AppUserRepository { get; }
}