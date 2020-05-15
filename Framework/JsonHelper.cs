using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Framework
{
    public class JsonHelper
    {
        public static T ToObject<T>(string str)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }

        public static string ToJson(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static string ReadJsonFile(string Path)
        {
            var json = File.ReadAllText(Path);
            return json;
        }

        public static JObject ReadJsonFile(string Path,string Object)
        {
            var json = File.ReadAllText(Path);
            return JObject.Parse(json);
        }

        /// <summary>
        /// 转化JsonResult
        /// </summary>
        /// <param name="JR"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string ConvertJsonResult(string JR, string Key)
        {
            Newtonsoft.Json.JsonSerializerSettings setting = new Newtonsoft.Json.JsonSerializerSettings();
            // 设置日期序列化的格式
            setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            dynamic modelDy = JsonConvert.DeserializeObject<dynamic>(JR);
            object ret = modelDy[Key];
            return ret.ToString();
        }

        /// <summary>
        /// 转化JsonResult
        /// </summary>
        /// <param name="JR"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string ConvertJsonResult(JsonResult JR, string Key)
        {
            object obj = JR.Data;
            Newtonsoft.Json.JsonSerializerSettings setting = new Newtonsoft.Json.JsonSerializerSettings();
            // 设置日期序列化的格式
            setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            string JsonStrValue = JsonConvert.SerializeObject(obj, setting);
            dynamic modelDy = JsonConvert.DeserializeObject<dynamic>(JsonStrValue);
            return modelDy[Key];
        }
        /// <summary>
        /// 转化JsonResult
        /// </summary>
        /// <param name="JR"></param>
        /// <returns></returns>
        public static string ConvertJsonResult(JsonResult JR)
        {
            object obj = JR.Data;
            JsonSerializerSettings setting = new JsonSerializerSettings();
            // 设置日期序列化的格式
            setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            string JsonStrValue = JsonConvert.SerializeObject(obj, setting);
            return JsonStrValue;
        }
    }
}
