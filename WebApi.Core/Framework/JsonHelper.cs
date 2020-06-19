using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Newtonsoft.Json;

namespace WebAPI.Core.Framework
{
    public static class JsonHelper
    {
        static JsonSerializerSettings jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DateFormatString = "yyyy-MM-dd HH:mm:ss" };

        /// <summary>
        ///  将单个DataRow转换成 Dictionary
        /// </summary>
        /// <param name="Row"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToJsonObject(this DataRow Row)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (DataColumn dataColumn in Row.Table.Columns)
            {
                if (Row[dataColumn.ColumnName].GetType().FullName == "System.Byte[]")
                {
                    dic.Add(dataColumn.ColumnName, Convert.ToBase64String((byte[])Row[dataColumn.ColumnName]));
                }
                else
                {
                    dic.Add(dataColumn.ColumnName, Row[dataColumn.ColumnName].ToString());
                }

            }
            return dic;
        }

        public static T ToObject<T>(string str)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str, jsonSetting);
        }

        public static string ToJson(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

    }
}
