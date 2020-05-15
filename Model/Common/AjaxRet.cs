using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Common
{
    [Serializable]
    public class AjaxRet : ModelBase
    {
        public AjaxRet() { }
        public AjaxRet(String retCode, String retMsg)
        {
            this.RetCode = retCode;
            this.RetMsg = retMsg;
        }
        public AjaxRet(String retCode, String retMsg, Object message)
        {
            this.RetCode = retCode;
            this.RetMsg = retMsg;
            this.Message = message;
        }

        public AjaxRet(String retCode, String retMsg, Object message, int total)
        {
            this.RetCode = retCode;
            this.RetMsg = retMsg;
            this.Message = message;
            this.Total = total;
        }

        public string RetCode { set; get; }
        public string RetMsg { set; get; }
        public object Message { get; set; }
        public int Total { get; set; }
        public string Redirect { get; set; }

    }
}
