namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxReportLineTotalInvoiceAmountMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaxReportLines", "TotalInvoiceAmount", c => c.Decimal(precision: 16, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaxReportLines", "TotalInvoiceAmount");
        }
    }
}
