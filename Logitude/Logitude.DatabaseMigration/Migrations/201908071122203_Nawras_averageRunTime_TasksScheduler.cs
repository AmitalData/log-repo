namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nawras_averageRunTime_TasksScheduler : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TasksScheduler", "AverageRunTime", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TasksScheduler", "AverageRunTime");
        }
    }
}
