namespace WebApplication2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sellers
    {
        public int Id { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public int deliveryTime { get; set; }

        public int score { get; set; }

        public int serviceScore { get; set; }

        public int foodScore { get; set; }

        public int rankRate { get; set; }

        public int minPrice { get; set; }

        public int deliveryPrice { get; set; }

        public int ratingCount { get; set; }

        public int sellCount { get; set; }

        public string bulletin { get; set; }

        public string avatar { get; set; }

        public int DelFlag { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
