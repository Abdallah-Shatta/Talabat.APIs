using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.IRepositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetAsync(int id);
        Task<IReadOnlyList<T>> GetAllWithSpecsAsync(ISpecifications<T> spec);
        Task<T?> GetWithSpecsAsync(ISpecifications<T> spec);
    }
}
