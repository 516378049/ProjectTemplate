using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Runda.B2B.Framework
{
    public class SingleObj<T>
    {
        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object Locker = new object();
        //单例模式获取redis实例
        private static T obj = default(T);
        public static T getObj
        {
            get
            {
                if (obj == null)
                {
                    lock (Locker)
                    {
                        if (obj == null)
                        {
                            Type t =  typeof(T).GetType();
                            obj=(T)t.Assembly.CreateInstance(t.Name);
                        }
                    }
                }
                return obj;
            }
        }
    }
}