using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPI.Core.Models
{
    [Serializable]
    public class AppInfo:ModelBase
    {
        public int AppID { get; set; }
        public string AppName { get; set; }
        /// <summary>
        /// 应用简介
        /// </summary>
        public string AppDesc { get; set; }
        /// <summary>
        /// 应用加密Key
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 应用Url
        /// </summary>
        public string AppUrl { get; set; }
        /// <summary>
        /// 应用登记的服务器IP列表，以逗号开始、分隔、结尾。例：,127.0.0.1,127.0.0.2,
        /// </summary>
        public string AppIPList { get; set; }
        /// <summary>
        /// 应用开发负责人
        /// </summary>
        public string DevUser { get; set; }
        /// <summary>
        /// 应用产品负责人
        /// </summary>
        public string ProductUser { get; set; }
        public int Status { get; set; }
        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime WriteTime { get; set; }
    }
}
