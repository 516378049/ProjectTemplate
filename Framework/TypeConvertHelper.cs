using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    /// <summary>
    /// 类型转换
    /// </summary>
    public class TypeConvertHelper
    {
        /// <summary>
        /// string to int
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt(string str, int defaultValue = 0)
        {
            int result = 0;
            bool success = int.TryParse(str, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// string to decimal
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(string str, decimal defaultValue = 0)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }

            decimal result;
            bool success = decimal.TryParse(str, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// string to datetime
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(string str, DateTime defaultValue)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }

            DateTime result;
            bool success = DateTime.TryParse(str, out result);

            return success ? result : defaultValue;
        }
    }
}
