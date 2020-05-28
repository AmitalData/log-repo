namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nawras_IsInternallyDefined_SchedulerProcedure : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SchedulerProcedure", "IsInternallyDefined", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SchedulerProcedure", "IsInternallyDefined");
        }
    }
}
