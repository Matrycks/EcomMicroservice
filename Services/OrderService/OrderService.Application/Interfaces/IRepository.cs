using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrderService.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T?> GetAsync(int entityId);
        public Task<T> AddAsync(T entity);
        public IEnumerable<T> Query(Expression<Func<T, bool>> predicate);
        public Task DeleteAsync(int entityId);
    }
}