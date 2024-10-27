using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class Specifications<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Filter { get; set; } = null!;
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();

        public Specifications() 
        {
            // filters may not exist and includes are initialized with empty list
        }
        public Specifications(Expression<Func<T, bool>> filter)
        {
            // if filters exist and includes are initialized with empty list
            Filter = filter;
        }
        public Specifications(List<Expression<Func<T, object>>> includes)
        {
            // filters may not exist and includes are assigned by constructor parameter list
            Includes = includes;
        }
        public Specifications(Expression<Func<T, bool>> filter, List<Expression<Func<T, object>>> includes)
        {
            // if filters exist and includes are assigned by constructor parameter list
            Filter = filter;
            Includes = includes;
        }
    }
}
