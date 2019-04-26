namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCardDetailsIToShipmentPickUpDeliveryPackageTable_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentPickUpDeliveryPackages", "Make", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPickUpDeliveryPackages", "Model", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPickUpDeliveryPackages", "Year", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPickUpDeliveryPackages", "Color", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPickUpDeliveryPackages", "ChassisNumber", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPickUpDeliveryPackages", "RegistrationNumber", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPickUpDeliveryPackages", "CountryId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.ShipmentPickUpDeliveryPackages", "CountryId");
            AddForeignKey("dbo.ShipmentPickUpDeliveryPackages", "CountryId", "dbo.Countries", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShipmentPickUpDeliveryPackages", "CountryId", "dbo.Countries");
            DropIndex("dbo.ShipmentPickUpDeliveryPackages", new[] { "CountryId" });
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "CountryId");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "RegistrationNumber");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "ChassisNumber");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "Color");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "Year");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "Model");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "Make");
        }
    }
}
