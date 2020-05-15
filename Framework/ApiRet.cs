using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    [Serializable]
    public class ApiRet
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
