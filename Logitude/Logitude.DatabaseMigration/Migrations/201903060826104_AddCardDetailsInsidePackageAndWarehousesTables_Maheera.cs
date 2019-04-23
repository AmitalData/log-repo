namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCardDetailsInsidePackageAndWarehousesTables_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InsideShipmentPackages", "Make", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.InsideShipmentPackages", "Model", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.InsideShipmentPackages", "Year", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.InsideShipmentPackages", "Color", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.InsideShipmentPackages", "ChassisNumber", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.InsideShipmentPackages", "RegistrationNumber", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.InsideShipmentPackages", "CountryId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseEntryPackages", "Make", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseEntryPackages", "Model", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseEntryPackages", "Year", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseEntryPackages", "Color", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseEntryPackages", "ChassisNumber", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseEntryPackages", "RegistrationNumber", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseEntryPackages", "CountryId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleasePackages", "Make", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseReleasePackages", "Model", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseReleasePackages", "Year", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseReleasePackages", "Color", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseReleasePackages", "ChassisNumber", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseReleasePackages", "RegistrationNumber", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseReleasePackages", "CountryId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.InsideShipmentPackages", "CountryId");
            CreateIndex("dbo.WarehouseEntryPackages", "CountryId");
            CreateIndex("dbo.WarehouseReleasePackages", "CountryId");
            AddForeignKey("dbo.InsideShipmentPackages", "CountryId", "dbo.Countries", "Id");
            AddForeignKey("dbo.WarehouseEntryPackages", "CountryId", "dbo.Countries", "Id");
            AddForeignKey("dbo.WarehouseReleasePackages", "CountryId", "dbo.Countries", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WarehouseReleasePackages", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.WarehouseEntryPackages", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.InsideShipmentPackages", "CountryId", "dbo.Countries");
            DropIndex("dbo.WarehouseReleasePackages", new[] { "CountryId" });
            DropIndex("dbo.WarehouseEntryPackages", new[] { "CountryId" });
            DropIndex("dbo.InsideShipmentPackages", new[] { "CountryId" });
            DropColumn("dbo.WarehouseReleasePackages", "CountryId");
            DropColumn("dbo.WarehouseReleasePackages", "RegistrationNumber");
            DropColumn("dbo.WarehouseReleasePackages", "ChassisNumber");
            DropColumn("dbo.WarehouseReleasePackages", "Color");
            DropColumn("dbo.WarehouseReleasePackages", "Year");
            DropColumn("dbo.WarehouseReleasePackages", "Model");
            DropColumn("dbo.WarehouseReleasePackages", "Make");
            DropColumn("dbo.WarehouseEntryPackages", "CountryId");
            DropColumn("dbo.WarehouseEntryPackages", "RegistrationNumber");
            DropColumn("dbo.WarehouseEntryPackages", "ChassisNumber");
            DropColumn("dbo.WarehouseEntryPackages", "Color");
            DropColumn("dbo.WarehouseEntryPackages", "Year");
            DropColumn("dbo.WarehouseEntryPackages", "Model");
            DropColumn("dbo.WarehouseEntryPackages", "Make");
            DropColumn("dbo.InsideShipmentPackages", "CountryId");
            DropColumn("dbo.InsideShipmentPackages", "RegistrationNumber");
            DropColumn("dbo.InsideShipmentPackages", "ChassisNumber");
            DropColumn("dbo.InsideShipmentPackages", "Color");
            DropColumn("dbo.InsideShipmentPackages", "Year");
            DropColumn("dbo.InsideShipmentPackages", "Model");
            DropColumn("dbo.InsideShipmentPackages", "Make");
        }
    }
}
