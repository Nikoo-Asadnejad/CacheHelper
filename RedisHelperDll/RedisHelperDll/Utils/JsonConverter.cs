using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RedisHelperDll.Utils
{
  public static class JsonConverter
  {
    public static string Serialize<T>(this T obj)
    => JsonConvert.SerializeObject(obj);

    public static T Deserialize<T>(this string json)
    => JsonConvert.DeserializeObject<T>(json);

  }
}
