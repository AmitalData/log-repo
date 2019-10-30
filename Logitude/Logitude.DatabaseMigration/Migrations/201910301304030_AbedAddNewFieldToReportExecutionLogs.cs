namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddNewFieldToReportExecutionLogs : DbMigration
    {
        public override void Up()
        {

            AddColumn("dbo.ReportExecutionLogs", "RetryNumber", c => c.Int(nullable: false));
            AddColumn("dbo.ReportExecutionLogs", "StartDate", c => c.DateTime());
            AddColumn("dbo.ReportExecutionLogs", "ExecutedByServerName", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReportExecutionLogs", "ExecutedByServerName");
            DropColumn("dbo.ReportExecutionLogs", "StartDate");
            DropColumn("dbo.ReportExecutionLogs", "RetryNumber");
        }
    }
}
