namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nawras1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SchedulerProcedure", "IsInternallyDefined", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SchedulerProcedure", "IsInternallyDefined", c => c.Boolean(nullable: false));
        }
    }
}
