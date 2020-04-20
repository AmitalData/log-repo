namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddReleaseNumberToShipmentPackage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentPackages", "WarehouseReleaseNumber", c => c.String(maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShipmentPackages", "WarehouseReleaseNumber");
        }
    }
}
