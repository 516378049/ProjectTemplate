namespace WebApplication2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class foods
    {
        public int Id { get; set; }

        public string name { get; set; }

        public int price { get; set; }

        public string oldPrice { get; set; }

        public string description { get; set; }

        public int sellCount { get; set; }

        public int rating { get; set; }

        public string info { get; set; }

        public string icon { get; set; }

        public string image { get; set; }

        public int DelFlag { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
