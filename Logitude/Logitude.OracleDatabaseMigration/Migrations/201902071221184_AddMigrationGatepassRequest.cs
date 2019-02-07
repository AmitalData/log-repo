namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationGatepassRequest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.GatepassRequests",
                c => new
                    {
                        MasterCourierId = c.String(nullable: false, maxLength: 15, unicode: false),
                        GatepassNumber = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        OriginSiteCode = c.String(maxLength: 17, unicode: false),
                        UpdateCode = c.String(maxLength: 2, unicode: false),
                        DesignateSiteCode = c.String(maxLength: 17, unicode: false),
                        TransportationTypeCode = c.String(maxLength: 4, unicode: false),
                        GatepassRequestStatus = c.String(maxLength: 2, unicode: false),
                        CustomsUpdateDateTime = c.DateTime(precision: 7),
                    })
                .PrimaryKey(t => new { t.MasterCourierId, t.GatepassNumber })
                .ForeignKey("Customs.SiteLookups", t => t.DesignateSiteCode)
                .ForeignKey("Customs.SiteLookups", t => t.OriginSiteCode)
                .ForeignKey("Customs.TransferCargoMethodTypes", t => t.TransportationTypeCode)
                .ForeignKey("Customs.UpdateCodes", t => t.UpdateCode)
                .Index(t => t.OriginSiteCode)
                .Index(t => t.UpdateCode)
                .Index(t => t.DesignateSiteCode)
                .Index(t => t.TransportationTypeCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.GatepassRequests", "UpdateCode", "Customs.UpdateCodes");
            DropForeignKey("Customs.GatepassRequests", "TransportationTypeCode", "Customs.TransferCargoMethodTypes");
            DropForeignKey("Customs.GatepassRequests", "OriginSiteCode", "Customs.SiteLookups");
            DropForeignKey("Customs.GatepassRequests", "DesignateSiteCode", "Customs.SiteLookups");
            DropIndex("Customs.GatepassRequests", new[] { "TransportationTypeCode" });
            DropIndex("Customs.GatepassRequests", new[] { "DesignateSiteCode" });
            DropIndex("Customs.GatepassRequests", new[] { "UpdateCode" });
            DropIndex("Customs.GatepassRequests", new[] { "OriginSiteCode" });
            DropTable("Customs.GatepassRequests");
        }
    }
}
