
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisHelperDll.Utils
{
  public class RedisHelperConfigurator
  {
    public static void Configure(IServiceCollection services , IConfiguration configuration)
    {
      //services.AddTransient<IRequestService, RequestService>();
      //services.AddTransient<IHttpService,HttpServices.Service.HttpService >();

      services.AddStackExchangeRedisCache(options =>
      {
        options.Configuration = configuration.GetConnectionString("Redis");
        options.InstanceName =  configuration.GetSection("ApplicationName").Value;
      }
      );
    }
  }
}
