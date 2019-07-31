namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteDownloadINIFileMenuButtonMigration : DbMigration
    {
        public override void Up()
        {
            Sql("delete from menubuttons where eventcode='INIDL' and menubuttongroupid=(select id from menubuttongroups where objecttableid =(select id from objecttables where name='openformatreport'))");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JournalMoreDatas", "TaxReportStatusCode", c => c.String(maxLength: 1, unicode: false));
            AddColumn("dbo.JournalMoreDatas", "TaxReportId", c => c.String(maxLength: 15, unicode: false));
            DropForeignKey("dbo.WarehouseReleasePackages", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.WarehouseEntryPackages", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.ShipmentPickUpDeliveryPackages", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.ShipmentPackages", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.InsideShipmentPackages", "CountryId", "dbo.Countries");
            DropIndex("dbo.WarehouseReleasePackages", new[] { "CountryId" });
            DropIndex("dbo.WarehouseEntryPackages", new[] { "CountryId" });
            DropIndex("dbo.ShipmentPickUpDeliveryPackages", new[] { "CountryId" });
            DropIndex("dbo.ShipmentPackages", new[] { "CountryId" });
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
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "CountryId");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "RegistrationNumber");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "ChassisNumber");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "Color");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "Year");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "Model");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "Make");
            DropColumn("dbo.JournalMoreDatas", "IsLedgerCreated");
            DropColumn("dbo.ShipmentPackages", "CountryId");
            DropColumn("dbo.ShipmentPackages", "RegistrationNumber");
            DropColumn("dbo.ShipmentPackages", "ChassisNumber");
            DropColumn("dbo.ShipmentPackages", "Color");
            DropColumn("dbo.ShipmentPackages", "Year");
            DropColumn("dbo.ShipmentPackages", "Model");
            DropColumn("dbo.ShipmentPackages", "Make");
            DropColumn("dbo.InsideShipmentPackages", "CountryId");
            DropColumn("dbo.InsideShipmentPackages", "RegistrationNumber");
            DropColumn("dbo.InsideShipmentPackages", "ChassisNumber");
            DropColumn("dbo.InsideShipmentPackages", "Color");
            DropColumn("dbo.InsideShipmentPackages", "Year");
            DropColumn("dbo.InsideShipmentPackages", "Model");
            DropColumn("dbo.InsideShipmentPackages", "Make");
            DropColumn("dbo.Automations", "Code");
            DropColumn("dbo.PackageTypes", "IsVehicle");
            CreateIndex("dbo.JournalMoreDatas", "TaxReportStatusCode");
            CreateIndex("dbo.JournalMoreDatas", "TaxReportId");
            AddForeignKey("dbo.JournalMoreDatas", "TaxReportStatusCode", "dbo.TaxReportStatuses", "Code");
            AddForeignKey("dbo.JournalMoreDatas", "TaxReportId", "dbo.TaxReports", "Id");
        }
    }
}
