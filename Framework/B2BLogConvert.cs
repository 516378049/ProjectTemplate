using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
   public class B2BLogConvert
    {
        public static B2BWebExEntity ParseWebEx(Exception ex)
        {
            return new B2BWebExEntity(ex);
        }

        public static string ParseFullDatePath(string startPath, string fileName)
        {
            return startPath + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString() + "\\" + DateTime.Now.Day.ToString() + "\\" + fileName;
        }
    }
}
