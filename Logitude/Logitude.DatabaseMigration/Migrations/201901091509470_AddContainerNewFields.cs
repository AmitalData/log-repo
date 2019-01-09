namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContainerNewFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "HasContainerException", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShipmentPackages", "ETD", c => c.DateTime());
            AddColumn("dbo.ShipmentPackages", "ETA", c => c.DateTime());
            AddColumn("dbo.ShipmentPackages", "Routing", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.ShipmentPackages", "VoyageTripNumber", c => c.String(maxLength: 10, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShipmentPackages", "VoyageTripNumber");
            DropColumn("dbo.ShipmentPackages", "Routing");
            DropColumn("dbo.ShipmentPackages", "ETA");
            DropColumn("dbo.ShipmentPackages", "ETD");
            DropColumn("dbo.Shipments", "HasContainerException");
        }
    }
}
