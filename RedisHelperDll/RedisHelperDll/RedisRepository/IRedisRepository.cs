using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DbSyncer.Services.RedisDataAccessor
{
    public interface IRedisRepository<T>
    {
        Task InsertAsync(T model);
        Task InsertRangeAsync(List<T> models);
        Task DeleteAsync(T model);
        Task<T> GetAsync(string id);
        Task<object> GetAsync(Expression<Func<T, bool>> query, Expression<Func<T, object>> selector);
        Task<List<object>> GetListAsync(Expression<Func<T, bool>> query,Expression<Func<T, object>> selector);
        Task Update(T model);
        Task UpdateRange(List<T> models);
    }
}
