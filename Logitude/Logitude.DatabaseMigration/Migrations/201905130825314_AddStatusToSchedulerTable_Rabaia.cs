namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusToSchedulerTable_Rabaia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TasksScheduler", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TasksScheduler", "Status");
        }
    }
}
