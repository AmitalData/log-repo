namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFreightPayerAddressId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "FreightPayerAddressId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.Shipments", "FreightPayerAddressId");
            AddForeignKey("dbo.Shipments", "FreightPayerAddressId", "dbo.Addresses", "Id");

            Sql("update Shipments set FreightPayerId = ShipperId, FreightPayerAddressId = ShipperAddressId where FreightPrepaidCollectId = 'P'");
            Sql("update Shipments set FreightPayerId = AgentId  , FreightPayerAddressId = AgentAddressId where FreightPrepaidCollectId = 'C'");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Shipments", "FreightPayerAddressId", "dbo.Addresses");
            DropIndex("dbo.Shipments", new[] { "FreightPayerAddressId" });
            DropColumn("dbo.Shipments", "FreightPayerAddressId");
        }
    }
}
