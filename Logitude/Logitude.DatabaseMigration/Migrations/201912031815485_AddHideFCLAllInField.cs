namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHideFCLAllInField : DbMigration
    {
        public override void Up()
        {            
            AddColumn("dbo.Tenants", "HideFCLAllIn", c => c.Boolean(nullable: false));

            Sql("update Tenants set HideFCLAllIn = IsHybrid");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tenants", "HideFCLAllIn");
        }
    }
}
