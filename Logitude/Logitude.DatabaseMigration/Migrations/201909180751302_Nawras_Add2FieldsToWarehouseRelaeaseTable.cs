namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nawras_Add2FieldsToWarehouseRelaeaseTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WarehouseReleases", "TotalVolumetricWeight", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            AddColumn("dbo.WarehouseReleases", "Ratio", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WarehouseReleases", "Ratio");
            DropColumn("dbo.WarehouseReleases", "TotalVolumetricWeight");
        }
    }
}
