using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EF.EF_ExAttr
{
    public class sellersEx : sellers
    {
        public sellersEx()
        {
        }
        public sellersEx(sellers sellers)
        {
            ExAttr.SynchronizationProperties(sellers, this);
        }
        public List<supports> supports { get; set; }
    }

    public class goodsEx : goods
    {
        public goodsEx()
        {
        }
        public goodsEx(goods goods)
        {
            ExAttr.SynchronizationProperties(goods, this);
        }
        public List<foodsEx> foods { get; set; }
    }

    public class foodsEx : foods
    {
        public foodsEx()
        {
        }
        public foodsEx(foods foods)
        {
            ExAttr.SynchronizationProperties(foods, this);
        }
        public List<ratings> ratings { get; set; }
    }
    
   
    public class OrderInfoEx : OrderInfo
    {
        public OrderInfoEx()
        {
        }
        public OrderInfoEx(OrderInfo OrderInfo)
        {
            ExAttr.SynchronizationProperties(OrderInfo, this);
        }
        public List<OrderDetailsInfo> OrderDetailsInfo { get; set; }
    }

    public class ExAttr
    {
        public static void SynchronizationProperties(object src, object des)
        {
            Type srcType = src.GetType();
            object val;
            foreach (var item in srcType.GetProperties())
            {
                val = item.GetValue(src);
                item.SetValue(des, val);

            }
        }
    }

}
