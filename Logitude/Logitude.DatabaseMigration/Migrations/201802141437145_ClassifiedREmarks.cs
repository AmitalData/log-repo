namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClassifiedREmarks : DbMigration
    {
        public override void Up()
        {
            //AddColumn("Customs.SupplierInvoiceItems", "ClasifiedRemarks", c => c.String(maxLength: 1024));
        }
        
        public override void Down()
        {
            DropColumn("Customs.SupplierInvoiceItems", "ClasifiedRemarks");
        }
    }
}
