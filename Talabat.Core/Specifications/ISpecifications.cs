using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Filter { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }
    }
}
