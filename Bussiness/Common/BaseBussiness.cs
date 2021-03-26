/******************************************************************
 * 功能描述：业务层基类，所有Rule类均需继承该基类
 * 创建时间：2015-03-04
 * 作者：changchun
 * 版本：1.0
 * 修订描述：
 * 最后修订日期：2020-1-13
 ******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Bussiness.Common
{
    public abstract class BussinessBase : MarshalByRefObject, IDisposable
    {
        /// <summary>
        /// 用于组装条件参数的方法，返回数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressions"></param>
        /// <returns></returns>
        protected Expression<Func<T, bool>>[] CreateBoolExpressions<T>(params Expression<Func<T, bool>>[] expressions)
        {
            return expressions;
        }

        protected static Expression<Func<T, bool>>[] CreateBoolExpressionsStatic<T>(params Expression<Func<T, bool>>[] expressions)
        {
            return expressions;
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
