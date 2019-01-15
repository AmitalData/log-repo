namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewField_SoftwareVersion_FullAccountingSettings : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.BIReports", "AGGridOptionsXML", c => c.String());
            AddColumn("dbo.FullAccountingSettings", "SoftwareVersion", c => c.String(maxLength: 100, unicode: false));
            //AddColumn("dbo.WarehouseEntries", "ChargeableWeightUnitCode", c => c.String(maxLength: 3, unicode: false));
            //AddColumn("dbo.WarehouseEntryPackages", "VolumetricWeight", c => c.Double());
            //AddColumn("dbo.WarehouseReleasePackages", "VolumetricWeight", c => c.Double());
            //AddColumn("dbo.WarehouseReleases", "ChargeableWeightUnitCode", c => c.String(maxLength: 3, unicode: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.WarehouseReleases", "ChargeableWeightUnitCode");
            //DropColumn("dbo.WarehouseReleasePackages", "VolumetricWeight");
            //DropColumn("dbo.WarehouseEntryPackages", "VolumetricWeight");
            //DropColumn("dbo.WarehouseEntries", "ChargeableWeightUnitCode");
            DropColumn("dbo.FullAccountingSettings", "SoftwareVersion");
            //DropColumn("dbo.BIReports", "AGGridOptionsXML");
        }
    }
}
