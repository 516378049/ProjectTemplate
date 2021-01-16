using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.orderMeal
{
    public class QueryParams:ModelBase
    {
        
    }
    #region 查询传参 
    /// <summary>
    /// 订单查询参数
    /// </summary>
    public class QueryOrderInfoParams : ModelBase
    {
        public int userId { get; set; }
        public int sellerId { get; set; }

        public string sortCreateTime { get; set; } //按照时间排序规则 默认倒序
        public bool timeEquals { get; set; } //查询时间是否为闭区间 默认为false非闭区间
        public DateTime startTime { get; set; }
        /// <summary>
        /// 1、向下拉(down)取查询当前时间以前订单，向上(up)拉取订单，查询当前时间以后订单
        /// </summary>
        public string slipAction { get; set; }
        public int count { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 订单明细状态
        /// </summary>
        public string DetailStatus { get; set; }
        public string OrderNum { get; set; }
    }
    /// <summary>
    /// 改变订单状态参数
    /// </summary>
    public class ChangeStatusParams : ModelBase
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        public int OrderId { get; set; }
    }
    #endregion


}
