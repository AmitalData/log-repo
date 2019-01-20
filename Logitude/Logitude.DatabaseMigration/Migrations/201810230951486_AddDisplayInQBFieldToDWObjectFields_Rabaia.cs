namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDisplayInQBFieldToDWObjectFields_Rabaia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DWObjectFields", "DisplayInQueryBuilder", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DWObjectFields", "DisplayInQueryBuilder");
        }
    }
}
