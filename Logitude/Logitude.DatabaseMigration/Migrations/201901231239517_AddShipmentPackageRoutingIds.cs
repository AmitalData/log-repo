namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShipmentPackageRoutingIds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentPackages", "RoutingIds", c => c.String(maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShipmentPackages", "RoutingIds");
        }
    }
}
