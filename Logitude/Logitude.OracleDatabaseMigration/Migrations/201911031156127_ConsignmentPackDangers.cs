namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConsignmentPackDangers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.ConsignmentPackDangers",
                c => new
                    {
                        DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        ConsignmentNumber = c.Int(nullable: false),
                        LineNumber = c.Int(nullable: false),
                        DangerousLineNo = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        UNCode = c.String(maxLength: 4, unicode: false),
                        DangerousGoodsPackingReqCode = c.String(maxLength: 3, unicode: false),
                        FlashpointTemperature = c.String(maxLength: 8, unicode: false),
                        StorageTemperature = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => new { t.DeclarationId, t.ConsignmentNumber, t.LineNumber, t.DangerousLineNo })
                .ForeignKey("Customs.ConsignmentPackages", t => new { t.DeclarationId, t.ConsignmentNumber, t.LineNumber })
                .ForeignKey("Customs.DangerousGoodsPackingReqs", t => t.DangerousGoodsPackingReqCode)
                .Index(t => new { t.DeclarationId, t.ConsignmentNumber, t.LineNumber })
                .Index(t => t.DangerousGoodsPackingReqCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.ConsignmentPackDangers", "DangerousGoodsPackingReqCode", "Customs.DangerousGoodsPackingReqs");
            DropForeignKey("Customs.ConsignmentPackDangers", new[] { "DeclarationId", "ConsignmentNumber", "LineNumber" }, "Customs.ConsignmentPackages");
            DropIndex("Customs.ConsignmentPackDangers", new[] { "DangerousGoodsPackingReqCode" });
            DropIndex("Customs.ConsignmentPackDangers", new[] { "DeclarationId", "ConsignmentNumber", "LineNumber" });
            DropTable("Customs.ConsignmentPackDangers");
        }
    }
}
