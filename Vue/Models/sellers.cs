//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vue.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class sellers
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Nullable<int> deliveryTime { get; set; }
        public Nullable<decimal> score { get; set; }
        public Nullable<decimal> serviceScore { get; set; }
        public Nullable<decimal> foodScore { get; set; }
        public Nullable<decimal> rankRate { get; set; }
        public Nullable<decimal> minPrice { get; set; }
        public Nullable<decimal> deliveryPrice { get; set; }
        public Nullable<int> ratingCount { get; set; }
        public Nullable<int> sellCount { get; set; }
        public string bulletin { get; set; }
        public string avatar { get; set; }
        public Nullable<int> DelFlag { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public List<supports> supports { get; set; }
    }
}
