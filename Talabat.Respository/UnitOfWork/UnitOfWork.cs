using Talabat.Core.Entities;
using Talabat.Core.IRepositories;
using Talabat.Core.IUnitOfWork;
using Talabat.Respository.Data;
using Talabat.Respository.Repositories;

namespace Talabat.Respository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<Type, object>();
        }
        public IGenericRepository<T> GetRepository<T>() where T : BaseEntity
        {
            if( _repositories.ContainsKey(typeof(T)))
                return (IGenericRepository<T>) _repositories[typeof(T)];

            var repository = new GenericRepository<T>(_dbContext);
            _repositories.Add(typeof(T), repository);
            return repository;
        }

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
        public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();
    }
}
