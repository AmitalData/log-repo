namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddARInvoiceStockIdToARInvoice_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ARInvoices", "ARInvoiceStockId", c => c.String(maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ARInvoiceStockLines", "ARInvoiceStockId", "dbo.ARInvoiceStocks");
        }
    }
}
