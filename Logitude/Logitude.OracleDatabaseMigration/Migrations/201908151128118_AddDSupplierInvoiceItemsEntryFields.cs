namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDSupplierInvoiceItemsEntryFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.SupplierInvoiceItems", "MarksAndNumbers", c => c.String(maxLength: 512, unicode: false));
            AddColumn("Customs.SupplierInvoiceItems", "PackageQuantity", c => c.Int());
            AddColumn("Customs.SupplierInvoiceItems", "Weight", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("Customs.SupplierInvoiceItems", "Weight");
            DropColumn("Customs.SupplierInvoiceItems", "PackageQuantity");
            DropColumn("Customs.SupplierInvoiceItems", "MarksAndNumbers");
        }
    }
}
