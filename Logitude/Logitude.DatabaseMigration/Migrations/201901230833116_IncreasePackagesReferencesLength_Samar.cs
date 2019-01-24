namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreasePackagesReferencesLength_Samar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InsideShipmentPackages", "Reference1", c => c.String(maxLength: 2000, unicode: false));
            AlterColumn("dbo.InsideShipmentPackages", "Reference2", c => c.String(maxLength: 2000, unicode: false));
            AlterColumn("dbo.InsideShipmentPackages", "Reference3", c => c.String(maxLength: 2000, unicode: false));
            AlterColumn("dbo.InsideShipmentPackages", "Reference4", c => c.String(maxLength: 2000, unicode: false));
            AlterColumn("dbo.ShipmentPackages", "Reference1", c => c.String(maxLength: 2000, unicode: false));
            AlterColumn("dbo.ShipmentPackages", "Reference2", c => c.String(maxLength: 2000, unicode: false));
            AlterColumn("dbo.ShipmentPackages", "Reference3", c => c.String(maxLength: 2000, unicode: false));
            AlterColumn("dbo.ShipmentPackages", "Reference4", c => c.String(maxLength: 2000, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ShipmentPackages", "Reference4", c => c.String(maxLength: 250, unicode: false));
            AlterColumn("dbo.ShipmentPackages", "Reference3", c => c.String(maxLength: 250, unicode: false));
            AlterColumn("dbo.ShipmentPackages", "Reference2", c => c.String(maxLength: 250, unicode: false));
            AlterColumn("dbo.ShipmentPackages", "Reference1", c => c.String(maxLength: 250, unicode: false));
            AlterColumn("dbo.InsideShipmentPackages", "Reference4", c => c.String(maxLength: 250, unicode: false));
            AlterColumn("dbo.InsideShipmentPackages", "Reference3", c => c.String(maxLength: 250, unicode: false));
            AlterColumn("dbo.InsideShipmentPackages", "Reference2", c => c.String(maxLength: 250, unicode: false));
            AlterColumn("dbo.InsideShipmentPackages", "Reference1", c => c.String(maxLength: 250, unicode: false));
        }
    }
}
