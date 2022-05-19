using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using RedisHelperDll.RedisExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace RedisHelperDll.Attributes
{
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
  public class RedisCachedAttribute : Attribute, IAsyncActionFilter
  {
    private readonly int _expireTime;
    public RedisCachedAttribute(int? expireTime = 60)
    {
      _expireTime = expireTime.Value;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      

      IDistributedCache cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();

      string cacheKey = GenerateKeyFromRequestUrl(context.HttpContext.Request);
      string cacheData = cache.GetRecordAsync<string>(cacheKey).Result;

      string statusCodeCacheKey = cacheKey + "-statusCode";
      int statusCode = cache.GetRecordAsync<int>(statusCodeCacheKey).Result;

      if (string.IsNullOrEmpty(cacheData))
      {
        
        var contentResult = new ContentResult
        {
          Content = cacheData,
          ContentType = "application/json;charset=UTF-8",
          StatusCode = statusCode

        };
        context.Result = contentResult;
        return;
      }
      else
      {
        var executedContect = await next();
        if(executedContect.Result is OkObjectResult okObjectResult )
        {
          await cache.SetRecordAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_expireTime));
          await cache.SetRecordAsync(statusCodeCacheKey, executedContect.HttpContext.Response.StatusCode, TimeSpan.FromSeconds(_expireTime));
        }
      }
      

    }

    private string GenerateKeyFromRequestUrl(HttpRequest context)
     => context.Host + context.Path + context.QueryString;
    
  }
}
