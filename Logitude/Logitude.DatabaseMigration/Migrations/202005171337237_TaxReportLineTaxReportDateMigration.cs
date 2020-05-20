namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxReportLineTaxReportDateMigration : DbMigration
    {
        public override void Up()
        {
          
            AddColumn("dbo.TaxReportLines", "TaxReportDate", c => c.DateTime());

        }
        
        public override void Down()
        {
            AddColumn("dbo.Reports", "ExcelOnly", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReportExecutionLogs", "ExcelOnly", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ShipmentPickUpDeliveries", "PickUpDeliveryNumber", c => c.String(nullable: false, maxLength: 20, unicode: false));
            DropColumn("dbo.TaxReportLines", "TaxReportDate");
            DropColumn("dbo.Reports", "DisablePreview");
            DropColumn("dbo.ReportExecutionLogs", "DisablePreview");
        }
    }
}
