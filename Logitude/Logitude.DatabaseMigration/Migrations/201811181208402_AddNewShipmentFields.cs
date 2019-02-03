namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewShipmentFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "BasicFreightId", c => c.String(maxLength: 1, unicode: false));
            AddColumn("dbo.Shipments", "DestinationPortChargesId", c => c.String(maxLength: 1, unicode: false));
            AddColumn("dbo.Shipments", "DestinationHaulageChargesId", c => c.String(maxLength: 1, unicode: false));
            AddColumn("dbo.Shipments", "AdditionalChargesId", c => c.String(maxLength: 1, unicode: false));
            AddColumn("dbo.Shipments", "FreightPayerId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.Shipments", "BasicFreightId");
            CreateIndex("dbo.Shipments", "DestinationPortChargesId");
            CreateIndex("dbo.Shipments", "DestinationHaulageChargesId");
            CreateIndex("dbo.Shipments", "AdditionalChargesId");
            CreateIndex("dbo.Shipments", "FreightPayerId");
            AddForeignKey("dbo.Shipments", "AdditionalChargesId", "dbo.PrepaidCollects", "Id");
            AddForeignKey("dbo.Shipments", "BasicFreightId", "dbo.PrepaidCollects", "Id");
            AddForeignKey("dbo.Shipments", "DestinationHaulageChargesId", "dbo.PrepaidCollects", "Id");
            AddForeignKey("dbo.Shipments", "DestinationPortChargesId", "dbo.PrepaidCollects", "Id");
            AddForeignKey("dbo.Shipments", "FreightPayerId", "dbo.Cards", "Id");


            Sql("update Shipments set BasicFreightId = FreightPrepaidCollectId");
            Sql("update Shipments set DestinationPortChargesId = OtherPrepaidCollectId");
            Sql("update Shipments set DestinationHaulageChargesId = OtherPrepaidCollectId");
            Sql("update Shipments set AdditionalChargesId = OtherPrepaidCollectId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shipments", "FreightPayerId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "DestinationPortChargesId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Shipments", "DestinationHaulageChargesId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Shipments", "BasicFreightId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Shipments", "AdditionalChargesId", "dbo.PrepaidCollects");
            DropIndex("dbo.Shipments", new[] { "FreightPayerId" });
            DropIndex("dbo.Shipments", new[] { "AdditionalChargesId" });
            DropIndex("dbo.Shipments", new[] { "DestinationHaulageChargesId" });
            DropIndex("dbo.Shipments", new[] { "DestinationPortChargesId" });
            DropIndex("dbo.Shipments", new[] { "BasicFreightId" });
            DropColumn("dbo.Shipments", "FreightPayerId");
            DropColumn("dbo.Shipments", "AdditionalChargesId");
            DropColumn("dbo.Shipments", "DestinationHaulageChargesId");
            DropColumn("dbo.Shipments", "DestinationPortChargesId");
            DropColumn("dbo.Shipments", "BasicFreightId");
        }
    }
}
