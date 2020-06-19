using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPI.Core.Models
{
    [Serializable]
    public class ApiQueryConfigInfo : ModelBase
    {
        /// <summary>
        /// IDX
        /// </summary>		
        public int IDX { get; set; }
        /// <summary>
        /// AppID
        /// </summary>		
        public int AppID { get; set; }
        /// <summary>
        /// AppCompanyID
        /// </summary>		
        public int AppCompanyID { get; set; }
        /// <summary>
        /// AppBrandID
        /// </summary>		
        public int AppBrandID { get; set; }
        /// <summary>
        /// AppSellCompanyID
        /// </summary>		
        public int AppSellCompanyID { get; set; }
        /// <summary>
        /// DelFlag
        /// </summary>		
        public int DelFlag { get; set; }
        /// <summary>
        /// WriteTime
        /// </summary>		
        public DateTime WriteTime { get; set; }
        /// <summary>
        /// UpdateTime
        /// </summary>		
        public DateTime UpdateTime { get; set; }

    }
}
