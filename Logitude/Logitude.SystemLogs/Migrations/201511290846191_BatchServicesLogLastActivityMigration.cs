namespace Logitude.SystemLogs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BatchServicesLogLastActivityMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BatchServicesLogs", "BatchServiceCode", c => c.String(nullable: false, maxLength: 40, unicode: false));
            AlterColumn("dbo.BatchServicesLogs", "LastActivity", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BatchServicesLogs", "LastActivity", c => c.String(unicode: false));
            AlterColumn("dbo.BatchServicesLogs", "BatchServiceCode", c => c.String(maxLength: 40, unicode: false));
        }
    }
}
