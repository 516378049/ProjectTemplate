namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SysUserInfo")]
    public partial class SysUserInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string NickName { get; set; }

        [StringLength(2)]
        public string Sex { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Mobile { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string WorkNum { get; set; }

        [StringLength(50)]
        public string LeaderWorkNum { get; set; }

        [StringLength(100)]
        public string JobName { get; set; }

        [StringLength(50)]
        public string QQ { get; set; }

        public int? CompanyID { get; set; }

        [StringLength(50)]
        public string CompanyName { get; set; }

        [StringLength(100)]
        public string LoginTicket { get; set; }

        [StringLength(1000)]
        public string Remark { get; set; }

        public int DelFlag { get; set; }

        public DateTime WriteTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
