namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shipmentnumber_length_15_to_40 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tickets", "ShipmentNumber", c => c.String(maxLength: 40, unicode: false));
            AlterColumn("dbo.APILogs", "Refrence", c => c.String(maxLength: 40, unicode: false));
            AlterColumn("dbo.Shipments", "ShipmentNumber", c => c.String(nullable: false, maxLength: 40, unicode: false));
            AlterColumn("dbo.Shipments", "ForwarderShipmentNumber", c => c.String(maxLength: 40, unicode: false));
            AlterColumn("dbo.Shipments", "CustomerShipmentNumber", c => c.String(maxLength: 40, unicode: false));
            AlterColumn("dbo.Shipments", "ComputedForwarderShipmentNumber", c => c.String(maxLength: 40, unicode: false));
            AlterColumn("dbo.ShipmentMasterDatas", "MasterShipmentNumber", c => c.String(maxLength: 40, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ShipmentMasterDatas", "MasterShipmentNumber", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Shipments", "ComputedForwarderShipmentNumber", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Shipments", "CustomerShipmentNumber", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Shipments", "ForwarderShipmentNumber", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Shipments", "ShipmentNumber", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.APILogs", "Refrence", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Tickets", "ShipmentNumber", c => c.String(maxLength: 15, unicode: false));
        }
    }
}
