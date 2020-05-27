namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameFieldExcelOnlyToDisablePreviewOnReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportExecutionLogs", "DisablePreview", c => c.Boolean(nullable: false));
            AddColumn("dbo.Reports", "DisablePreview", c => c.Boolean(nullable: false));
            DropColumn("dbo.ReportExecutionLogs", "ExcelOnly");
            DropColumn("dbo.Reports", "ExcelOnly");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reports", "ExcelOnly", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReportExecutionLogs", "ExcelOnly", c => c.Boolean(nullable: false));
            DropColumn("dbo.Reports", "DisablePreview");
            DropColumn("dbo.ReportExecutionLogs", "DisablePreview");
        }
    }
}
