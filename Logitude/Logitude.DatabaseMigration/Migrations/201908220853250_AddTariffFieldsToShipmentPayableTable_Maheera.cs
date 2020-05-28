namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTariffFieldsToShipmentPayableTable_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentPayables", "TariffId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ShipmentPayables", "TariffNumber", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShipmentPayables", "TariffNumber");
            DropColumn("dbo.ShipmentPayables", "TariffId");            
        }
    }
}
