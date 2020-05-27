namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamChangeTypeOfNameDescriptionTasksSchedulerToNVarChar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TasksScheduler", "Name", c => c.String(maxLength: 100));
            AlterColumn("dbo.TasksScheduler", "Description", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TasksScheduler", "Description", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.TasksScheduler", "Name", c => c.String(maxLength: 100, unicode: false));
        }
    }
}
