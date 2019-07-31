namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pendingChanges_Nawras : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SchedulerProcedure", "SearchFields", c => c.String(nullable: false, maxLength: 1000, unicode: false));
            AlterColumn("dbo.Tariffs", "StartDate", c => c.DateTime());
            AlterColumn("dbo.Tariffs", "ExpirationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tariffs", "ExpirationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tariffs", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SchedulerProcedure", "SearchFields", c => c.String());
        }
    }
}
