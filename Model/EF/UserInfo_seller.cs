//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class UserInfo_seller
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string icon { get; set; }
        public string email { get; set; }
        public string nick_name { get; set; }
        public string note { get; set; }
        public Nullable<System.DateTime> login_time { get; set; }
        public int DelFlag { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public Nullable<int> CreateUserId { get; set; }
        public Nullable<int> UpdateUserId { get; set; }
        public int sellers_Id { get; set; }
        public virtual sellers sellers { get; set; }
    }
}