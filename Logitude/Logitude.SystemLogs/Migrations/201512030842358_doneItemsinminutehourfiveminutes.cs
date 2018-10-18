namespace Logitude.SystemLogs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class doneItemsinminutehourfiveminutes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BatchServicesLogs", "DoneItemsInOneMinute", c => c.Int(nullable: false));
            AddColumn("dbo.BatchServicesLogs", "DoneItemsInFiveMinutes", c => c.Int(nullable: false));
            AddColumn("dbo.BatchServicesLogs", "DoneItemsInOneHour", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BatchServicesLogs", "DoneItemsInOneHour");
            DropColumn("dbo.BatchServicesLogs", "DoneItemsInFiveMinutes");
            DropColumn("dbo.BatchServicesLogs", "DoneItemsInOneMinute");
        }
    }
}
