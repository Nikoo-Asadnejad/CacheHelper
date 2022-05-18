using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using RedisHelperDll.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisHelperDll.RedisExtension
{
  public static class Cacher
  {
    public async static Task SetRecordAsync<T>(this IDistributedCache cache,
      string key , T data,
      TimeSpan? expirationDuration,
      TimeSpan? unusedExpirationDuration)
    {
      DistributedCacheEntryOptions options = GenerateCacheOptions(expirationDuration,unusedExpirationDuration).Result;
      cache.SetString(key, data.Serialize<T>() ,options);
    }

    public async static Task SetRecordsListAsync<T>(this IDistributedCache cache,
      Dictionary<string, T> records,
      TimeSpan? expirationDuration,
      TimeSpan? unusedExpirationDuration)
    {
      foreach(var record in records)
      {
         await cache.SetRecordAsync(record.Key, record.Value, expirationDuration, unusedExpirationDuration);
      }
    }

    public async static Task GetRecordAsync<T>(this IDistributedCache cache,
      string key, T data)
    => cache.GetStringAsync(key).Result.Deserialize<T>();

    public async static Task GetRecordsListAsync<T>(this IDistributedCache cache, Dictionary<string, T> records)
    {
      foreach (var record in records)
      {
        await cache.GetRecordAsync(record.Key, record.Value);
      }
    }


    private static async Task<DistributedCacheEntryOptions> GenerateCacheOptions(TimeSpan? expirationDuration , TimeSpan? unusedExpirationDuration)
    {
      DistributedCacheEntryOptions options = new();
      options.AbsoluteExpirationRelativeToNow = expirationDuration == null ? TimeSpan.FromSeconds(60) : expirationDuration;
      options.SlidingExpiration = unusedExpirationDuration;
      return options;
    }
  }
}
