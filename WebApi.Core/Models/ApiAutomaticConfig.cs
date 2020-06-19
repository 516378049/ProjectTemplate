using WebAPI.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPI.Core.Models
{
    public class ApiAutomaticConfig : ModelBase
    {
        public int IDX { get; set; }
        public string @Action { get; set; }
        /// <summary>
        /// 请求方法
        /// GET/POST
        /// </summary>
        public string Method { get; set; }
        public string Module { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// 入参
        /// [{ "Name": "QRCode", "DataType": "VARCHAR", "ParamType": "IN", "Required": true }, {...}, ...] 
        /// </summary>
        public string Params { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 返回类型
        /// Object|List
        /// </summary>
        public string ReturnType { get; set; }

        /// <summary>
        /// 返回结果映射
        /// [{ "RetCode": "-1", "RetMsg": "请输入编号"}, {...}, ...] 
        /// </summary>
        public string ResultMapping { get; set; }

        public int DelFlag { get; set; }
        public DateTime WriteTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public List<DynamicParam> GetDynamicParams()
        {
            if (string.IsNullOrWhiteSpace(Params))
            {
                return new List<DynamicParam>();
            }
            return JsonHelper.ToObject<List<DynamicParam>>(Params);
        }

        public List<DynamicResult> GetDynamicResult()
        {
            if (string.IsNullOrWhiteSpace(ResultMapping))
            {
                return new List<DynamicResult>();
            }
            return JsonHelper.ToObject<List<DynamicResult>>(ResultMapping);
        }

    }
}
