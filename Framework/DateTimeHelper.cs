using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class DateTimeHelper
    {
        public static DateTime GetMinValue()
        {
            return Convert.ToDateTime("1900-1-1");
        }


        /// <summary>
        /// 获取交货创建日期(传入日期往后一天，如遇周六周日顺延)
        /// </summary>
        /// <param name="dtDueDate"></param>
        /// <returns>返回格式 yyyyMMdd</returns>
        public static string GetDueDate(DateTime dtDueDate)
        {
            dtDueDate = dtDueDate.AddDays(1).Date;

            if (dtDueDate.DayOfWeek == DayOfWeek.Saturday || dtDueDate.DayOfWeek == DayOfWeek.Sunday)
            {
                return GetDueDate(dtDueDate);
            }
            else
            {
                return StringHelper.FormatDateTime(dtDueDate, "yyyyMMdd");
            }
        }

        /// <summary>
        /// 计算两个日期的时间间隔
        /// </summary>
        /// <param name="DateTime1">第一个日期和时间</param>
        /// <param name="DateTime2">第二个日期和时间</param>
        /// <returns></returns>
        public static int DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            int dateDiff = 0;

            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();

            dateDiff = ts.Days;

            return dateDiff;
        }
    }
}
