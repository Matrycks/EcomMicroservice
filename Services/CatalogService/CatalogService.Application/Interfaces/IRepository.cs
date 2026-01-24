using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace CatalogService.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T?> GetAsync(int entityId);
        public Task AddAsync(T entity);
        public Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> predicate);
        public Task DeleteAsync(int entityId);
    }
}