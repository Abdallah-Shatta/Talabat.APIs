using Talabat.Core.Entities;
using Talabat.Core.IRepositories;

namespace Talabat.Core.IUnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : BaseEntity;
        Task<int> SaveChangesAsync();
    }
}
