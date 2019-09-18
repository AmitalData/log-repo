namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nawras_AddFieldsToWarehouseReleaseTable : DbMigration
    {
        public override void Up()
        {


            AddColumn("dbo.WarehouseReleases", "FromPortId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "ToPortId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "CustomerAddressId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.WarehouseReleases", "FromPortId");
            CreateIndex("dbo.WarehouseReleases", "ToPortId");
            AddForeignKey("dbo.WarehouseReleases", "FromPortId", "dbo.Ports", "Id");
            AddForeignKey("dbo.WarehouseReleases", "ToPortId", "dbo.Ports", "Id");
        }
        
        public override void Down()
        {
         
            DropForeignKey("dbo.WarehouseReleases", "ToPortId", "dbo.Ports");
            DropForeignKey("dbo.WarehouseReleases", "FromPortId", "dbo.Ports");
            DropForeignKey("dbo.LogBoxTenantSettings", "Id", "dbo.Tenants");
         
            DropIndex("dbo.WarehouseReleases", new[] { "ToPortId" });
            DropIndex("dbo.WarehouseReleases", new[] { "FromPortId" });
      
            DropColumn("dbo.WarehouseReleases", "ToPortId");
           DropColumn("dbo.WarehouseReleases", "FromPortId");

        }
    }
}
