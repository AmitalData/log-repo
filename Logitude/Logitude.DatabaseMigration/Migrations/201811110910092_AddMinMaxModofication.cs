namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMinMaxModofication : DbMigration
    {
        public override void Up()
        {           
            AddColumn("dbo.QuoteCharges", "SaleMaxAmount", c => c.Double());
            AddColumn("dbo.ShipmentReceivables", "MaxAmount", c => c.Double());
            RenameColumn(table: "dbo.ShipmentReceivables", name: "QuoteSaleMinPrice", newName: "MinAmount");
        }

        public override void Down()
        {
            RenameColumn(table: "dbo.ShipmentReceivables", name: "MinAmount", newName: "QuoteSaleMinPrice");
            DropColumn("dbo.ShipmentReceivables", "MaxAmount");
            DropColumn("dbo.QuoteCharges", "SaleMaxAmount");
        }
    }
}
