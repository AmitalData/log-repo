namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddNewFieldsTruckerToWarehouseRlease : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WarehouseReleases", "TruckerId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "TruckerReference", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.WarehouseReleases", "TruckerId");
            AddForeignKey("dbo.WarehouseReleases", "TruckerId", "dbo.Cards", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WarehouseReleases", "TruckerId", "dbo.Cards");
            DropIndex("dbo.WarehouseReleases", new[] { "TruckerId" });
            DropColumn("dbo.WarehouseReleases", "TruckerReference");
            DropColumn("dbo.WarehouseReleases", "TruckerId");
        }
    }
}
