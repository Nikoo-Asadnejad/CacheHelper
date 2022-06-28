using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Redis.OM.Searching;
using System.Linq.Expressions;
using Redis.OM;

namespace RedisHelperDll.Repository;

public class RedisRepository<T> where T : class
{
    private readonly IConfiguration _config;
    public IRedisCollection<T> ModelCollection;
    public RedisRepository(IConfiguration config)
    {
        _config = config;
        var provider = new RedisConnectionProvider(_config["RedisStackConnection"]);
        var connection = provider.Connection;
        ModelCollection = provider.RedisCollection<T>();
        connection.CreateIndexAsync(typeof(T));
    }

    public async Task InsertAsync(T model)
     => await ModelCollection.InsertAsync(model);
    
    public async Task InsertRangeAsync(List<T> models)
    => models.ForEach(async model =>await  ModelCollection.InsertAsync(model));

    public async Task DeleteAsync(T model)
    => await ModelCollection.Delete(model);

  public async Task DeleteRangeAsync(List<T> models)
    => models.ForEach(model => ModelCollection.Delete(model));

  public async Task<T> FindAsync(string id)
    => await ModelCollection.FindByIdAsync(id);

    public async Task<object> GetAsync(Expression<Func<T, bool>> query,
         Expression<Func<T, object>> selector)
   =>  ModelCollection.Where(query).Select(selector).ToList().FirstOrDefault();

    public async Task<List<object>> GetListAsync(Expression<Func<T, bool>> query,
          Expression<Func<T, object>> selector)
    => ModelCollection.Where(query).Select(selector).ToList();

  public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> query = null)
    => query is null ? ModelCollection.ToList() : ModelCollection.Where(query).ToList();

  public async Task Update(T model)
    => await ModelCollection.Update(model);

    public async Task UpdateRange(List<T> models)
    =>  models.ForEach(async model => await ModelCollection.Update(model));




}
