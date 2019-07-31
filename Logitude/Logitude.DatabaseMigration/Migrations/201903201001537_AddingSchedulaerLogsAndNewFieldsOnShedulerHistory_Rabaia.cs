namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingSchedulaerLogsAndNewFieldsOnShedulerHistory_Rabaia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SchedulerLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        Log = c.String(unicode: false),
                        TaskId = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TasksScheduler", t => t.TaskId)
                .Index(t => t.TaskId);
            
            AddColumn("dbo.TaskSchedulerHistory", "LogType", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.TaskSchedulerHistory", "LogFirstLine", c => c.String(maxLength: 1000, unicode: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SchedulerLogs", "TaskId", "dbo.TasksScheduler");
            DropIndex("dbo.SchedulerLogs", new[] { "TaskId" });
            DropColumn("dbo.TaskSchedulerHistory", "LogFirstLine");
            DropColumn("dbo.TaskSchedulerHistory", "LogType");
            DropTable("dbo.SchedulerLogs");
        }
    }
}
