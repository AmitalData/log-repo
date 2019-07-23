namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShipmentFields_To_ComputedShipmentTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentComputedFields", "Commodity", c => c.String());
            AddColumn("dbo.ShipmentComputedFields", "FirstPickupLocation", c => c.String());
            AddColumn("dbo.ShipmentComputedFields", "ContainersNumbers", c => c.String());
            DropColumn("dbo.Shipments", "Commodity");
            DropColumn("dbo.Shipments", "FirstPickupLocation");
            DropColumn("dbo.Shipments", "ContainersNumbers");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shipments", "ContainersNumbers", c => c.String(maxLength: 1000));
            AddColumn("dbo.Shipments", "FirstPickupLocation", c => c.String(maxLength: 100));
            AddColumn("dbo.Shipments", "Commodity", c => c.String(maxLength: 15));
            DropColumn("dbo.ShipmentComputedFields", "ContainersNumbers");
            DropColumn("dbo.ShipmentComputedFields", "FirstPickupLocation");
            DropColumn("dbo.ShipmentComputedFields", "Commodity");
        }
    }
}
