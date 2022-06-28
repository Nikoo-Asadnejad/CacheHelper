using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedisHelperDll.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisHelperDll.Configurations
{
  public static class RedisHelperConfigurator
  {
    public static void InjectServices(IServiceCollection services, IConfiguration configuration)
    {
      services.AddStackExchangeRedisCache(options =>
      options.Configuration = configuration["RedisConnection"]);

      services.AddTransient(typeof(IRedisRepository<>), typeof(RedisRepository<>));
    }
  }
}
