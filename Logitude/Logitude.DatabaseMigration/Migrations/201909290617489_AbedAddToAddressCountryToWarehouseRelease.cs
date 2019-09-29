namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddToAddressCountryToWarehouseRelease : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.WarehouseReleases", "ToAddressCountryId");
            AddForeignKey("dbo.WarehouseReleases", "ToAddressCountryId", "dbo.Countries", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WarehouseReleases", "ToAddressCountryId", "dbo.Countries");
            DropIndex("dbo.WarehouseReleases", new[] { "ToAddressCountryId" });
        }
    }
}
