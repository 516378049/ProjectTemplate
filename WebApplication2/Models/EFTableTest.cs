namespace WebApplication2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EFTableTest")]
    public partial class EFTableTest
    {
        public int id { get; set; }

        [StringLength(100)]
        public string name { get; set; }
    }
}
