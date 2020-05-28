namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddExcelOnlyToReportExecutionLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportExecutionLogs", "ExcelOnly", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReportExecutionLogs", "ExcelOnly");
        }
    }
}
