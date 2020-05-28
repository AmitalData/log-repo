namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropSchedulerLogsTable_Rabaia : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SchedulerLogs", "HistoryId", "dbo.TaskSchedulerHistory");
            DropIndex("dbo.SchedulerLogs", new[] { "HistoryId" });
            DropTable("dbo.SchedulerLogs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SchedulerLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        HistoryId = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.SchedulerLogs", "HistoryId");
            AddForeignKey("dbo.SchedulerLogs", "HistoryId", "dbo.TaskSchedulerHistory", "Id");
        }
    }
}
