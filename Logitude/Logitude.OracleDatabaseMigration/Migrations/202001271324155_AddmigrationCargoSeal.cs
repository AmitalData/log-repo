namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddmigrationCargoSeal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.CargoSeals",
                c => new
                    {
                        CargoSealIdentifierId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SealNumber = c.String(nullable: false, maxLength: 35, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 512),
                        SealCompletenessStateCode = c.String(maxLength: 2, unicode: false),
                        SealTypeCode = c.String(maxLength: 3, unicode: false),
                        UpdateReasonCode = c.String(maxLength: 3, unicode: false),
                        UpdateTypeCode = c.String(maxLength: 2, unicode: false),
                    })
                .PrimaryKey(t => new { t.CargoSealIdentifierId, t.SealNumber })
                .ForeignKey("Customs.AmendmentTypes", t => t.UpdateTypeCode)
                .ForeignKey("Customs.CargoSealIdentifiers", t => t.CargoSealIdentifierId)
                .ForeignKey("Customs.SealCompleteness", t => t.SealCompletenessStateCode)
                .ForeignKey("Customs.SealTypes", t => t.SealTypeCode)
                .ForeignKey("Customs.SealUpdateReasonTypes", t => t.UpdateReasonCode)
                .Index(t => t.CargoSealIdentifierId)
                .Index(t => t.SealCompletenessStateCode)
                .Index(t => t.SealTypeCode)
                .Index(t => t.UpdateReasonCode)
                .Index(t => t.UpdateTypeCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.CargoSeals", "UpdateReasonCode", "Customs.SealUpdateReasonTypes");
            DropForeignKey("Customs.CargoSeals", "SealTypeCode", "Customs.SealTypes");
            DropForeignKey("Customs.CargoSeals", "SealCompletenessStateCode", "Customs.SealCompleteness");
            DropForeignKey("Customs.CargoSeals", "CargoSealIdentifierId", "Customs.CargoSealIdentifiers");
            DropForeignKey("Customs.CargoSeals", "UpdateTypeCode", "Customs.AmendmentTypes");
            DropIndex("Customs.CargoSeals", new[] { "UpdateTypeCode" });
            DropIndex("Customs.CargoSeals", new[] { "UpdateReasonCode" });
            DropIndex("Customs.CargoSeals", new[] { "SealTypeCode" });
            DropIndex("Customs.CargoSeals", new[] { "SealCompletenessStateCode" });
            DropIndex("Customs.CargoSeals", new[] { "CargoSealIdentifierId" });
            DropTable("Customs.CargoSeals");
        }
    }
}
