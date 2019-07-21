namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditSchedulerProcedure_Nawras : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SchedulerProcedure", "SearchFields", c => c.String(maxLength: 1000, unicode: false));
            AlterColumn("dbo.SchedulerProcedure", "Description", c => c.String(maxLength: 1000, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SchedulerProcedure", "Description", c => c.String(nullable: false, maxLength: 1000, unicode: false));
            AlterColumn("dbo.SchedulerProcedure", "SearchFields", c => c.String(nullable: false, maxLength: 1000, unicode: false));
        }
    }
}
