namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShipmentContainerStatusChanges : DbMigration
    {
        public override void Up()
        {            
            AddColumn("dbo.ShipmentContainerStatuses", "VesselName", c => c.String(maxLength: 35, unicode: false));
            AlterColumn("dbo.ShipmentContainerStatuses", "VoyageNumber", c => c.String(maxLength: 35, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ShipmentContainerStatuses", "VoyageNumber", c => c.String(maxLength: 15, unicode: false));
            DropColumn("dbo.ShipmentContainerStatuses", "VesselName");
        }
    }
}
