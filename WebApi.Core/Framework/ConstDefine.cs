using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPI.Core
{
    public class CoreConstDefine
    {
        #region 各逻辑层错误码统一前缀
        public const string ErrCode_Pre_DataAccess = "-101";
        public const string ErrCode_Pre_Business = "-102";
        public const string ErrCode_Pre_WebAPI = "-104";
        public const string ErrCode_Pre_AdminWeb = "-105";
        #endregion

        public const string RedisKeyPre = "Runda::WebAPI";
    }
}
