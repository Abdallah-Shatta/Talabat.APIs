﻿using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.IRepositories;
using Talabat.Core.Specifications;
using Talabat.Respository.Data;

namespace Talabat.Respository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecsAsync(ISpecifications<T> spec)
        {
            return await GetSpecifiedQuery(spec).ToListAsync();
        }

        public async Task<T?> GetWithSpecsAsync(ISpecifications<T> spec)
        {
            return await GetSpecifiedQuery(spec).FirstOrDefaultAsync();
        }
        public async Task<int> GetCountAsync(ISpecifications<T> spec)
        {
            return await GetSpecifiedQuery(spec).CountAsync();
        }
        // Private method to encapsulate the generation of the specified query
        // with SpecificationHandler class by its BuildQuery static method
        private IQueryable<T> GetSpecifiedQuery(ISpecifications<T> spec)
        {
            // BuildQuery takes initialQuery as first parameter which is [_dbContext.Set<T>()]
            return SpecificationsHandler<T>.BuildQuery(_dbContext.Set<T>(), spec);
        }

        public async Task AddAsync(T entity) => await _dbContext.AddAsync(entity);

        public void Update(T entity) => _dbContext.Update(entity);

        public void Delete(T entity) => _dbContext.Remove(entity);
    }
}
