namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCarsDBToShipmentPackagesDB_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentPackages", "Make", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPackages", "Model", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPackages", "Year", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPackages", "Color", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPackages", "ChassisNumber", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPackages", "RegistrationNumber", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPackages", "CountryId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.ShipmentPackages", "CountryId");
            AddForeignKey("dbo.ShipmentPackages", "CountryId", "dbo.Countries", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShipmentPackages", "CountryId", "dbo.Countries");
            DropIndex("dbo.ShipmentPackages", new[] { "CountryId" });
            DropColumn("dbo.ShipmentPackages", "CountryId");
            DropColumn("dbo.ShipmentPackages", "RegistrationNumber");
            DropColumn("dbo.ShipmentPackages", "ChassisNumber");
            DropColumn("dbo.ShipmentPackages", "Color");
            DropColumn("dbo.ShipmentPackages", "Year");
            DropColumn("dbo.ShipmentPackages", "Model");
            DropColumn("dbo.ShipmentPackages", "Make");
        }
    }
}
