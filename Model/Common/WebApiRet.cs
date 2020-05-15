/******************************************************************
 * 功能描述：WebAPI的统一返回类型
 * 创建时间：2015-03-04
 * 作者：Ray
 * 版本：1.0
 * 修订描述：
 * 最后修订日期：
 ******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Newtonsoft.Json;

namespace Models.Common
{
    public class WebApiRet : ModelBase
    {
        public string RetCode { set; get; }
        public string RetMsg { set; get; }
        public object Message { set; get; }
        public int Total { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
