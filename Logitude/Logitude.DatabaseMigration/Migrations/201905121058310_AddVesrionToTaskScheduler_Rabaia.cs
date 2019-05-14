namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVesrionToTaskScheduler_Rabaia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TasksScheduler", "Version", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TasksScheduler", "Version");
        }
    }
}
