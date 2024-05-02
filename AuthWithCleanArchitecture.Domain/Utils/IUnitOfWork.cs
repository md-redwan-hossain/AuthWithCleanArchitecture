using System.Data.Common;

namespace AuthWithCleanArchitecture.Domain.Utils;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    void Save();
    Task SaveAsync();
    Task<DbTransaction> BeginTransactionAsync();
}