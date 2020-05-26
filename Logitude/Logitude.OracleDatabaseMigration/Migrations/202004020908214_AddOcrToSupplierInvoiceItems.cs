namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOcrToSupplierInvoiceItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.SupplierInvoiceItems", "OcrHeight", c => c.Decimal(nullable: false, precision: 5, scale: 0));
            AddColumn("Customs.SupplierInvoiceItems", "OcrTop", c => c.Decimal(nullable: false, precision: 5, scale: 0));
            AddColumn("Customs.SupplierInvoiceItems", "OcrPageNumber", c => c.Decimal(nullable: false, precision: 3, scale: 0));
        }
        
        public override void Down()
        {
            DropColumn("Customs.SupplierInvoiceItems", "OcrPageNumber");
            DropColumn("Customs.SupplierInvoiceItems", "OcrTop");
            DropColumn("Customs.SupplierInvoiceItems", "OcrHeight");
        }
    }
}
