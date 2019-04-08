namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShipmentNumberToInvoiceStockLine_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ARInvoiceStockLines", "ShipmentNumber", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ARInvoiceStockLines", "ShipmentNumber");
        }
    }
}
