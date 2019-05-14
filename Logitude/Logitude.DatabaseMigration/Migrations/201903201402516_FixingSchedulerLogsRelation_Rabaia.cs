namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixingSchedulerLogsRelation_Rabaia : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SchedulerLogs", "TaskId", "dbo.TasksScheduler");
            DropIndex("dbo.SchedulerLogs", new[] { "TaskId" });
            AddColumn("dbo.SchedulerLogs", "HistoryId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            CreateIndex("dbo.SchedulerLogs", "HistoryId");
            AddForeignKey("dbo.SchedulerLogs", "HistoryId", "dbo.TaskSchedulerHistory", "Id");
            DropColumn("dbo.SchedulerLogs", "TaskId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SchedulerLogs", "TaskId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            DropForeignKey("dbo.SchedulerLogs", "HistoryId", "dbo.TaskSchedulerHistory");
            DropIndex("dbo.SchedulerLogs", new[] { "HistoryId" });
            DropColumn("dbo.SchedulerLogs", "HistoryId");
            CreateIndex("dbo.SchedulerLogs", "TaskId");
            AddForeignKey("dbo.SchedulerLogs", "TaskId", "dbo.TasksScheduler", "Id");
        }
    }
}
