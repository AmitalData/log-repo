namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddEntityIdToTasksSchedulerTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TasksScheduler", "EntityId", c => c.String(maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TasksScheduler", "EntityId");
        }
    }
}
