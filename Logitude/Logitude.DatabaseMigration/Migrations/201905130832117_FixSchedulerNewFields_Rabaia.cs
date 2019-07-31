namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSchedulerNewFields_Rabaia : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TasksScheduler", "Status", c => c.String(maxLength: 25, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TasksScheduler", "Status", c => c.String());
        }
    }
}
