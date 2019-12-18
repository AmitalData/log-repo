namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDSupplierInvoiceItemsEntryFieldsChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Customs.SupplierInvoiceItems", "MarksAndNumbers", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("Customs.SupplierInvoiceItems", "Weight", c => c.Decimal(precision: 15, scale: 3));
        }
        
        public override void Down()
        {
            AlterColumn("Customs.SupplierInvoiceItems", "Weight", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("Customs.SupplierInvoiceItems", "MarksAndNumbers", c => c.String(maxLength: 512, unicode: false));
        }
    }
}
