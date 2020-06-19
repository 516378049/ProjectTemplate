using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ThinkDev.Logging.Counter;
namespace WebAPI.Core
{
    public class CountManager
    {
        /// <summary>
        /// 针对API统计的计数器
        /// </summary>
        public static Counter ApiCounter { get { return CounterFactory.GetCounter("ApiCounter"); } }
    }
}
