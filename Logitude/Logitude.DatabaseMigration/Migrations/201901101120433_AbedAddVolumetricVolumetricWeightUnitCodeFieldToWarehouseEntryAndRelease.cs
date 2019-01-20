namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddVolumetricVolumetricWeightUnitCodeFieldToWarehouseEntryAndRelease : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WarehouseEntries", "ChargeableWeightUnitCode", c => c.String(maxLength: 3, unicode: false));
            AddColumn("dbo.WarehouseReleases", "ChargeableWeightUnitCode", c => c.String(maxLength: 3, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WarehouseReleases", "ChargeableWeightUnitCode");
            DropColumn("dbo.WarehouseEntries", "ChargeableWeightUnitCode");
        }
    }
}
