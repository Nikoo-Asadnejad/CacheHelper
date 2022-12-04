using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using RedisHelperDll.RedisExtension;
using RedisHelperDll.Utils;
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
  public class MemoryCacheAttribute : Attribute, IAsyncActionFilter
  {
    private readonly int _expireTime;
    public MemoryCacheAttribute(int expireTime = 60)
    {
      _expireTime = expireTime;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      

      IMemoryCache cache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();

      string cacheKey = GenerateKeyFromRequestUrl(context.HttpContext.Request);
      string cacheData =  cache.Get<string>(cacheKey);


      if (!string.IsNullOrEmpty(cacheData))
      {
        
        var contentResult = new ContentResult
        {
          Content = cacheData,
          ContentType = "application/json;charset=UTF-8",
        };
        context.Result = contentResult;
        return;
      }
      else
      {
        var executedContect = await next();
        if(executedContect.Result is OkObjectResult okObjectResult )
          cache.Set(cacheKey, okObjectResult.Value.Serialize(), TimeSpan.FromSeconds(_expireTime));
        
      }
      

    }

    private string GenerateKeyFromRequestUrl(HttpRequest context)
    {
      var key = context.Host + context.Path + context.QueryString;
      if (context.Body is not null && context.Method is not "GET")
        key += context.Body.GetHashCode();

      return key;
    }

     
    
  }
}
