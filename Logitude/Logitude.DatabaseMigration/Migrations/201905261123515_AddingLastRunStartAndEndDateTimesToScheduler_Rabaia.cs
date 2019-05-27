namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingLastRunStartAndEndDateTimesToScheduler_Rabaia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TasksScheduler", "LastRunEndTime", c => c.DateTime());
            AddColumn("dbo.TasksScheduler", "LastRunEndTimeUTC", c => c.DateTime());
            AddColumn("dbo.TasksScheduler", "LastRunStartTimeUTC", c => c.DateTime());
            AddColumn("dbo.TasksScheduler", "LastRunStartTime", c => c.DateTime());
            DropColumn("dbo.TasksScheduler", "LastRunTime");
            DropColumn("dbo.TasksScheduler", "LastRunTimeUTC");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TasksScheduler", "LastRunTimeUTC", c => c.DateTime());
            AddColumn("dbo.TasksScheduler", "LastRunTime", c => c.DateTime());
            DropColumn("dbo.TasksScheduler", "LastRunStartTime");
            DropColumn("dbo.TasksScheduler", "LastRunStartTimeUTC");
            DropColumn("dbo.TasksScheduler", "LastRunEndTimeUTC");
            DropColumn("dbo.TasksScheduler", "LastRunEndTime");
        }
    }
}
