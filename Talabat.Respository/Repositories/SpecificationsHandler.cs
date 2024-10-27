using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Respository.Repositories
{
    internal static class SpecificationsHandler<T> where T : BaseEntity
    {
        // A static method to apply specifications to a query
        public static IQueryable<T> BuildQuery (IQueryable<T> initialQuery, ISpecifications<T> spec)
        {
            var query = initialQuery;

            if(spec.Filter is not null)
                query = query.Where(spec.Filter);

            // Aggregate method has an overload that take an initial seed to start the aggregation which is [query]
            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            return query;
        }
    }
}
