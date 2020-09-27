namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedChangeFromPortForeignKeyToWarehouseRelease : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.WarehouseReleases", "FromPortId", "dbo.Ports");
            //Sql("update WarehouseReleases set FromPortId = null");
            //AddForeignKey("dbo.WarehouseReleases", "FromPortId", "dbo.Warehouses", "Id");
            //Sql("update WarehouseReleases set FromPortId = WarehouseId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WarehouseReleases", "FromPortId", "dbo.Warehouses");
            AddForeignKey("dbo.WarehouseReleases", "FromPortId", "dbo.Ports", "Id");
        }
    }
}
