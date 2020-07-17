using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Common
{
    public class RedisCacheHelper
    {
        private  ConnectionMultiplexer connection = null;
        public RedisCacheHelper(string RedisCacheConnectionString)
        {
            if (connection == null)
            {
                connection = ConnectionMultiplexer.Connect(RedisCacheConnectionString);
            }
        }

        public void SetString(string Key, string Value)
        {
            IDatabase cache = connection.GetDatabase();
            cache.StringSet(Key, Value);
        }

        public string GetString(string Key)
        {
            IDatabase cache = connection.GetDatabase();
            return cache.StringGet(Key);
        }

        public T GetObject<T>(string Key)
        {
            IDatabase cache = connection.GetDatabase();
            string result = cache.StringGet(Key);
            if (string.IsNullOrEmpty(result))
            {
                return default(T);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
        }

        public void RemoveCache(string Key)
        { 
            IDatabase cache = connection.GetDatabase();
            cache.KeyDelete(Key);
        }

        public void SetObject(string Key, object CacheObject)
        {
            IDatabase cache = connection.GetDatabase();
            cache.StringSet(Key, JsonConvert.SerializeObject(CacheObject));
        }

    }
}
