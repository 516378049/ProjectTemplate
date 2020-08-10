namespace WebApplication2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ratings
    {
        public int Id { get; set; }

        public string username { get; set; }

        public int rateTime { get; set; }

        public int deliveryTime { get; set; }

        public int score { get; set; }

        public int rateType { get; set; }

        public string text { get; set; }

        public string avatar { get; set; }

        public int DelFlag { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
