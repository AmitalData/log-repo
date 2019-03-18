namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddNewFieldToTableTasksScheduler : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TasksScheduler", "SchedulerDetailsXML", c => c.String());
            AddColumn("dbo.TasksScheduler", "Type", c => c.String(maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TasksScheduler", "Type");
            DropColumn("dbo.TasksScheduler", "SchedulerDetailsXML");
        }
    }
}
