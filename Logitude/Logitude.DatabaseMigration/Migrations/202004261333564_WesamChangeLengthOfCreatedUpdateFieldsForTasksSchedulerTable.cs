namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamChangeLengthOfCreatedUpdateFieldsForTasksSchedulerTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TasksScheduler", "CreatedBy", c => c.String(nullable: false, maxLength: 40, unicode: false));
            AlterColumn("dbo.TasksScheduler", "UpdatedBy", c => c.String(nullable: false, maxLength: 40, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TasksScheduler", "UpdatedBy", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.TasksScheduler", "CreatedBy", c => c.String(nullable: false, maxLength: 15, unicode: false));
        }
    }
}
