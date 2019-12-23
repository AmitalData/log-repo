namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameAirlineAreaToCarrierArea_Samar : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AirlineAreas", "AirlineId", "dbo.Airlines");
            DropForeignKey("dbo.AirlineAreasPorts", "AirlineAreaId", "dbo.AirlineAreas");
            
            DropIndex("dbo.AirlineAreas", new[] { "AirlineId" });
            DropIndex("dbo.AirlineAreasPorts", new[] { "AirlineAreaId" });

            RenameTable(name: "dbo.AirlineAreas", newName: "CarrierAreas");
            RenameTable(name: "dbo.AirlineAreasPorts", newName: "CarrierAreasPorts");

            RenameColumn("dbo.CarrierAreas", "AirlineId", "CarrierId");
            RenameColumn("dbo.CarrierAreasPorts", "AirlineAreaId", "CarrierAreaId");

            CreateIndex("dbo.CarrierAreas", "CarrierId");
            CreateIndex("dbo.CarrierAreasPorts", "CarrierAreaId");

            AddForeignKey("dbo.CarrierAreas", "CarrierId", "dbo.Cards", "Id");
            AddForeignKey("dbo.CarrierAreasPorts", "CarrierAreaId", "dbo.CarrierAreas", "Id");            
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AirlineAreasPorts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AirlineAreaId = c.String(nullable: false, maxLength: 128),
                        PortId = c.String(maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        AddedDate = c.DateTime(),
                        AddedByUserId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
           
            AddColumn("dbo.CarrierAreas", "AirlineId", c => c.String(nullable: false, maxLength: 15, unicode: false));            
            DropForeignKey("dbo.CarrierAreasPorts", "PortId", "dbo.Ports");
            DropForeignKey("dbo.CarrierAreasPorts", "CarrierAreaId", "dbo.CarrierAreas");
            DropForeignKey("dbo.CarrierAreasPorts", "AddedByUserId", "dbo.Users");
            DropForeignKey("dbo.CarrierAreas", "CarrierId", "dbo.Cards");            
            DropIndex("dbo.CarrierAreasPorts", new[] { "AddedByUserId" });
            DropIndex("dbo.CarrierAreasPorts", new[] { "PortId" });
            DropIndex("dbo.CarrierAreasPorts", new[] { "CarrierAreaId" });
            DropIndex("dbo.CarrierAreas", new[] { "CarrierId" });            
            DropColumn("dbo.CarrierAreas", "CarrierId");            
            DropTable("dbo.CarrierAreasPorts");
            CreateIndex("dbo.AirlineAreasPorts", "AddedByUserId");
            CreateIndex("dbo.AirlineAreasPorts", "PortId");
            CreateIndex("dbo.AirlineAreasPorts", "AirlineAreaId");
            CreateIndex("dbo.CarrierAreas", "AirlineId");
            AddForeignKey("dbo.AirlineAreasPorts", "PortId", "dbo.Ports", "Id");
            AddForeignKey("dbo.AirlineAreasPorts", "AirlineAreaId", "dbo.AirlineAreas", "Id");
            AddForeignKey("dbo.AirlineAreasPorts", "AddedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.AirlineAreas", "AirlineId", "dbo.Airlines", "Id");
            RenameTable(name: "dbo.CarrierAreas", newName: "AirlineAreas");
        }
    }
}
