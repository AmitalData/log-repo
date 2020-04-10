namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDisplayDocumentsAndEventsToTenant : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tenants", "DisplayDocumentsAndEvents", c => c.Boolean(nullable: false));
            //DropColumn("dbo.SharedLogisticsSettings", "DisplayDocumentsAndEvents");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.SharedLogisticsSettings", "DisplayDocumentsAndEvents", c => c.Boolean(nullable: false));
            DropColumn("dbo.Tenants", "DisplayDocumentsAndEvents");
        }
    }
}
