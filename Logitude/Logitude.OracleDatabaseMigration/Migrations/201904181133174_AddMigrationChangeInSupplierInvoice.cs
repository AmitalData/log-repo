namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationChangeInSupplierInvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.SupplierInvoices", "ChangeInSupplierInvoice", c => c.String(maxLength: 1, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("Customs.SupplierInvoices", "ChangeInSupplierInvoice");
        }
    }
}
