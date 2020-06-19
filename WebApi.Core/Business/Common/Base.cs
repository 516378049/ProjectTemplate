using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPI.Core.Business
{
    public class Base : MarshalByRefObject, IDisposable
    {

        public string GetErrCode(string code)
        {
            return CoreConstDefine.ErrCode_Pre_Business + code;
        }


        /// <summary>
        /// 用于远程对象
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }

        #region "释放内存"
        /// <summary>
        /// 释放内存
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        /// <summary>
        /// 释放内存具体操作
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {

        }
        #endregion


    }
}
