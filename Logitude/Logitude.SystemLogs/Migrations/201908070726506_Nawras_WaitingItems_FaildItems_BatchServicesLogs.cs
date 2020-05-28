namespace Logitude.SystemLogs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nawras_WaitingItems_FaildItems_BatchServicesLogs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BatchServicesLogs", "WaitingItems", c => c.Int(nullable: false));
            AddColumn("dbo.BatchServicesLogs", "FailedItems", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BatchServicesLogs", "FailedItems");
            DropColumn("dbo.BatchServicesLogs", "WaitingItems");
        }
    }
}
