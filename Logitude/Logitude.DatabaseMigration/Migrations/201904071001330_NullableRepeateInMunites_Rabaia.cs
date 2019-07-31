namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableRepeateInMunites_Rabaia : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TasksScheduler", "RepeatInMinutes", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TasksScheduler", "RepeatInMinutes", c => c.Int(nullable: false));
        }
    }
}
