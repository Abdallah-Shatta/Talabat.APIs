using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Respository.Repositories
{
    internal static class SpecificationsHandler<T> where T : BaseEntity
    {
        // A static method to apply specifications to a query
        public static IQueryable<T> BuildQuery(IQueryable<T> initialQuery, ISpecifications<T> spec)
        {
            var query = initialQuery;

            if (spec.Filter is not null)
                query = query.Where(spec.Filter);

            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);

            else if (spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);

            if(spec.IsPaginationAllowed)
                query = query.Skip(spec.Skip).Take(spec.Take);

            // Aggregate method has an overload that take an initial seed to start the aggregation which is [query]
            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            return query;
        }
    }
}
