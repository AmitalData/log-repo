namespace Logitude.SystemLogs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddFieldReasonToFailedLoginLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FailedLoginLogs", "Reason", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FailedLoginLogs", "Reason");
        }
    }
}
