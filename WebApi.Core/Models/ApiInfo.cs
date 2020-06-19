using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPI.Core.Models
{
    [Serializable]
    public class ApiInfo : ModelBase
    {
        public int ApiID { get; set; }
        public string Module { get; set; }
        public string ApiName { get; set; }
        public string ApiDesc { get; set; }
        public string ApiType { get; set; }
        public string DevUser { get; set; }
        public string ApiVersion { get; set; }
        public bool IsUse { get; set; }
        /// <summary>
        /// 应用验证类型
        /// 0：不做验证
        /// 1：MD5验证（验证参数+key的MD5值是否一致）
        /// </summary>
        public int ValidateType { get; set; }
        public string UrlExample { get; set; }
        public DateTime WriteTime { get; set; }
    }


    public class ApiCountInfo:ModelBase
    {
        public string Module { get; set; }
        public string ApiName { get; set; }
        public string AppID { get; set; }
        public string ApiID { get; set; }
        public string RetCode { get; set; }
        public int Count { get; set; }
        public DateTime CountTime { get; set; }
    }

    [Serializable]
    public class App_ApiRelation:ModelBase
    {
        public int RID { get; set; }
        public int AppID { get; set; }
        public int ApiID { get; set; }
        public bool IsUse { get; set; }
        public string DownInfo { get; set; }
        public DateTime WriteTime { get; set; }
    }
}
