namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddARInvoiceRegionalTax : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ARInvoices", "RegionalTaxId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ARInvoices", "RegionalTaxPercentage", c => c.Double());
            CreateIndex("dbo.ARInvoices", "RegionalTaxId");
            AddForeignKey("dbo.ARInvoices", "RegionalTaxId", "dbo.VatTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ARInvoices", "RegionalTaxId", "dbo.VatTypes");
            DropIndex("dbo.ARInvoices", new[] { "RegionalTaxId" });
            DropColumn("dbo.ARInvoices", "RegionalTaxPercentage");
            DropColumn("dbo.ARInvoices", "RegionalTaxId");
        }
    }
}
