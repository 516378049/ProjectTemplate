using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Runda.WebAPI.Core
{
    public class ResultInfo<T>
    {
        public ResultInfo(bool hasError, string message, string hRESULT, T infoObj)
        {
            HasError = hasError;
            Message = message;
            HRESULT = hRESULT;
            InfoObj = infoObj;
        }

        public ResultInfo() { }

        public bool HasError { get; set; }
        public string Message { get; set; }
        public string HRESULT { get; set; }
        public T InfoObj { get; set; }

        public override string ToString()
        {
            return HasError.ToString() + "," + Message + "," + HRESULT + "," + (InfoObj == null ? "NULL" : InfoObj.ToString());
        }
    }
}
