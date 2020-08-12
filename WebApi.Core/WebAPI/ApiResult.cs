using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;
namespace WebAPI.Core.WebAPI
{
    public class ApiResult
    {
        public string RetCode { get; set; }
        public string RetMsg { get; set; }
        public object Message { get; set; }
        public int Total { get; set; }
        public string ToJson()
        {
            
            JsonSerializerSettings setting = new JsonSerializerSettings();
            // 设置日期序列化的格式
            setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(this, setting);
        }
    }
}