namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddToShipmentComputedFields_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentComputedFields", "OperationallyClosedByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ShipmentComputedFields", "NumberOfDeliveries", c => c.Int());
            AddColumn("dbo.ShipmentComputedFields", "ImportDeclarationDate", c => c.DateTime());
            AddColumn("dbo.ShipmentComputedFields", "ImportDeclarationNumber", c => c.String());
            AddColumn("dbo.ShipmentComputedFields", "LastPickupETA", c => c.DateTime());
            AddColumn("dbo.ShipmentComputedFields", "LastPickupETD", c => c.DateTime());
            AddColumn("dbo.ShipmentComputedFields", "LastPickupATA", c => c.DateTime());
            AddColumn("dbo.ShipmentComputedFields", "LastPickupATD", c => c.DateTime());
            AddColumn("dbo.ShipmentComputedFields", "DeliveryToCity", c => c.String());
            AddColumn("dbo.ShipmentComputedFields", "DeliveryToPortId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ShipmentComputedFields", "ContainsDangerousGoods", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShipmentComputedFields", "DeliveryFrom", c => c.String());
            AddColumn("dbo.ShipmentComputedFields", "DeliveryTo", c => c.String());
            AddColumn("dbo.ShipmentComputedFields", "PickupFrom", c => c.String());
            AddColumn("dbo.ShipmentComputedFields", "PickupTo", c => c.String());            
            CreateIndex("dbo.ShipmentComputedFields", "OperationallyClosedByUserId");
            CreateIndex("dbo.ShipmentComputedFields", "DeliveryToPortId");
            AddForeignKey("dbo.ShipmentComputedFields", "DeliveryToPortId", "dbo.Ports", "Id");
            AddForeignKey("dbo.ShipmentComputedFields", "OperationallyClosedByUserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShipmentComputedFields", "OperationallyClosedByUserId", "dbo.Users");
            DropForeignKey("dbo.ShipmentComputedFields", "DeliveryToPortId", "dbo.Ports");
            DropIndex("dbo.ShipmentComputedFields", new[] { "DeliveryToPortId" });
            DropIndex("dbo.ShipmentComputedFields", new[] { "OperationallyClosedByUserId" });           
            DropColumn("dbo.ShipmentComputedFields", "PickupTo");
            DropColumn("dbo.ShipmentComputedFields", "PickupFrom");
            DropColumn("dbo.ShipmentComputedFields", "DeliveryTo");
            DropColumn("dbo.ShipmentComputedFields", "DeliveryFrom");
            DropColumn("dbo.ShipmentComputedFields", "ContainsDangerousGoods");
            DropColumn("dbo.ShipmentComputedFields", "DeliveryToPortId");
            DropColumn("dbo.ShipmentComputedFields", "DeliveryToCity");
            DropColumn("dbo.ShipmentComputedFields", "LastPickupATD");
            DropColumn("dbo.ShipmentComputedFields", "LastPickupATA");
            DropColumn("dbo.ShipmentComputedFields", "LastPickupETD");
            DropColumn("dbo.ShipmentComputedFields", "LastPickupETA");
            DropColumn("dbo.ShipmentComputedFields", "ImportDeclarationNumber");
            DropColumn("dbo.ShipmentComputedFields", "ImportDeclarationDate");
            DropColumn("dbo.ShipmentComputedFields", "NumberOfDeliveries");
            DropColumn("dbo.ShipmentComputedFields", "OperationallyClosedByUserId");            
        }
    }
}
