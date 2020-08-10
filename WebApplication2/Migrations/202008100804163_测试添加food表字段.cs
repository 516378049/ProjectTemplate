namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 测试添加food表字段 : DbMigration
    {
        public override void Up()
        {
            AddColumn("foods", "testfiled", X => X.String(true));
        }
        
        public override void Down()
        {
            DropColumn("foods", "testfiled");
        }
    }
}
