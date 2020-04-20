namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDisplayDocumentsAndEvents : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.SharedLogisticsSettings", "DisplayDocumentsAndEvents", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.SharedLogisticsSettings", "DisplayDocumentsAndEvents");
        }
    }
}
