using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RedisHelperDll.Repository;

//This interface uses Redis Stack , and works with redis.om packages
//It will help to store datas in redis stack in documents format , and applys queries on them
//The model which will be passed to this interface should have been decoreted with redis.om attributes
public interface IRedisRepository<T>
{
  Task InsertAsync(T model);
  Task InsertRangeAsync(List<T> models);
  Task DeleteAsync(T model);
  Task DeleteRangeAsync(T model);
  Task<T> FindAsync(string id);
  Task<object> GetAsync(Expression<Func<T, bool>> query, Expression<Func<T, object>> selector);
  Task<T> GetAsync(Expression<Func<T, bool>> query);
  Task<List<object>> GetListAsync(Expression<Func<T, bool>> query, Expression<Func<T, object>> selector);
  Task<List<T>> GetListAsync(Expression<Func<T, bool>> query = null);
  Task Update(T model);
  Task UpdateRange(List<T> models);
}

