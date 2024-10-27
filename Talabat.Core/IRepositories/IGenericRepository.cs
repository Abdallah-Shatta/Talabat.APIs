using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.IRepositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(int id);
        Task<IEnumerable<T>> GetAllWithSpecsAsync(ISpecifications<T> spec);
        Task<T?> GetWithSpecsAsync(ISpecifications<T> spec);
    }
}
