using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPI.Core.Models
{
    [Serializable]
    public class ApiCompanyConfigInfo : ModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        public int IDX { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int AppID { get; set; }

        /// <summary>
        /// App的使用公司ID
        /// </summary>
        public int AppCompanyID { get; set; }

        /// <summary>
        /// SIMS部门ID
        /// </summary>
        public string SectionID { get; set; }

        /// <summary>
        /// B2B用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// B2B收货地址
        /// </summary>
        public int SendID { get; set; }

        /// <summary>
        /// 送达方ID
        /// </summary>
        public int SendCompanyID { get; set; }

        /// <summary>
        /// 默认卖方ID
        /// </summary>
        public int DefaultSellCompanyID { get; set; }

        /// <summary>
        /// 售达方ID
        /// </summary>
        public int AgentCompanyID { get; set; }

        /// <summary>
        /// 是否为打包客户，1：打包客户  2：非打包客户（目前该字段的作用为当是非打包客户时，通过DNA上传订单中无合约的物料进入异常订单列表）
        /// </summary>
        public string PackagingCustomers { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int DelFlag { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime WriteTime { get; set; }

        /// <summary>
        /// 记录修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
