namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMinMaxModofication2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentPayables", "QuoteCostMaxAmount", c => c.Double());
            RenameColumn(table: "dbo.ShipmentPayables", name: "QuoteCostMinPrice", newName: "QuoteCostMinAmount");
            RenameColumn(table: "dbo.ShipmentReceivables", name: "MinAmount", newName: "QuoteSaleMinAmount");
            RenameColumn(table: "dbo.ShipmentReceivables", name: "MaxAmount", newName: "QuoteSaleMaxAmount");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.ShipmentReceivables", name: "QuoteSaleMaxAmount", newName: "MaxAmount");
            RenameColumn(table: "dbo.ShipmentReceivables", name: "QuoteSaleMinAmount", newName: "MinAmount");
            RenameColumn(table: "dbo.ShipmentPayables", name: "QuoteCostMinAmount", newName: "QuoteCostMinPrice");
            DropColumn("dbo.ShipmentPayables", "QuoteCostMaxAmount");
        }
    }
}
