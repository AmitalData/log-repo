namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddVolumetricWeighFieldToWarehouseEntryAndReleasePackage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WarehouseEntryPackages", "VolumetricWeight", c => c.Double());
            AddColumn("dbo.WarehouseReleasePackages", "VolumetricWeight", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WarehouseReleasePackages", "VolumetricWeight");
            DropColumn("dbo.WarehouseEntryPackages", "VolumetricWeight");
        }
    }
}
