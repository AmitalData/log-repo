namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersLayoutDirectionMigration : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Users", "LayoutDirection", c => c.String(maxLength: 3, unicode: false));
        }

        public override void Down()
        {
            //DropColumn("dbo.Users", "LayoutDirection");
        }
    }
}
