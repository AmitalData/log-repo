namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CargoSealsChanges : DbMigration
    {
        public override void Up()
        {
            DropIndex("Customs.CargoSeals", new[] { "SealCompletenessStateCode" });
            DropIndex("Customs.CargoSeals", new[] { "SealTypeCode" });
            DropIndex("Customs.CargoSeals", new[] { "UpdateReasonCode" });
            DropIndex("Customs.CargoSeals", new[] { "UpdateTypeCode" });
            DropIndex("Customs.CargoSealIdentifiers", new[] { "ImporterId" });
            DropIndex("Customs.CargoSealIdentifiers", new[] { "CargoIdentifierTypeCode" });
            AlterColumn("Customs.CargoSeals", "SealCompletenessStateCode", c => c.String(nullable: false, maxLength: 2, unicode: false));
            AlterColumn("Customs.CargoSeals", "SealTypeCode", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AlterColumn("Customs.CargoSeals", "UpdateReasonCode", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AlterColumn("Customs.CargoSeals", "UpdateTypeCode", c => c.String(nullable: false, maxLength: 2, unicode: false));
            AlterColumn("Customs.CargoSealIdentifiers", "CargoRowNumber", c => c.String(nullable: false, maxLength: 9, unicode: false));
            AlterColumn("Customs.CargoSealIdentifiers", "UpdateDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("Customs.CargoSealIdentifiers", "ImporterId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("Customs.CargoSealIdentifiers", "CargoIdentifierTypeCode", c => c.String(nullable: false, maxLength: 4, unicode: false));
            AlterColumn("Customs.CargoSealIdentifiers", "CargoIdentifierKey1", c => c.String(nullable: false, maxLength: 35, unicode: false));
            AlterColumn("Customs.CargoSealIdentifiers", "CargoIdentifierKey2", c => c.String(nullable: false, maxLength: 35, unicode: false));
            CreateIndex("Customs.CargoSeals", "SealCompletenessStateCode");
            CreateIndex("Customs.CargoSeals", "SealTypeCode");
            CreateIndex("Customs.CargoSeals", "UpdateReasonCode");
            CreateIndex("Customs.CargoSeals", "UpdateTypeCode");
            CreateIndex("Customs.CargoSealIdentifiers", "ImporterId");
            CreateIndex("Customs.CargoSealIdentifiers", "CargoIdentifierTypeCode");
        }
        
        public override void Down()
        {
            DropIndex("Customs.CargoSealIdentifiers", new[] { "CargoIdentifierTypeCode" });
            DropIndex("Customs.CargoSealIdentifiers", new[] { "ImporterId" });
            DropIndex("Customs.CargoSeals", new[] { "UpdateTypeCode" });
            DropIndex("Customs.CargoSeals", new[] { "UpdateReasonCode" });
            DropIndex("Customs.CargoSeals", new[] { "SealTypeCode" });
            DropIndex("Customs.CargoSeals", new[] { "SealCompletenessStateCode" });
            AlterColumn("Customs.CargoSealIdentifiers", "CargoIdentifierKey2", c => c.String(maxLength: 35, unicode: false));
            AlterColumn("Customs.CargoSealIdentifiers", "CargoIdentifierKey1", c => c.String(maxLength: 35, unicode: false));
            AlterColumn("Customs.CargoSealIdentifiers", "CargoIdentifierTypeCode", c => c.String(maxLength: 4, unicode: false));
            AlterColumn("Customs.CargoSealIdentifiers", "ImporterId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("Customs.CargoSealIdentifiers", "UpdateDate", c => c.DateTime(precision: 7));
            AlterColumn("Customs.CargoSealIdentifiers", "CargoRowNumber", c => c.String(maxLength: 9, unicode: false));
            AlterColumn("Customs.CargoSeals", "UpdateTypeCode", c => c.String(maxLength: 2, unicode: false));
            AlterColumn("Customs.CargoSeals", "UpdateReasonCode", c => c.String(maxLength: 3, unicode: false));
            AlterColumn("Customs.CargoSeals", "SealTypeCode", c => c.String(maxLength: 3, unicode: false));
            AlterColumn("Customs.CargoSeals", "SealCompletenessStateCode", c => c.String(maxLength: 2, unicode: false));
            CreateIndex("Customs.CargoSealIdentifiers", "CargoIdentifierTypeCode");
            CreateIndex("Customs.CargoSealIdentifiers", "ImporterId");
            CreateIndex("Customs.CargoSeals", "UpdateTypeCode");
            CreateIndex("Customs.CargoSeals", "UpdateReasonCode");
            CreateIndex("Customs.CargoSeals", "SealTypeCode");
            CreateIndex("Customs.CargoSeals", "SealCompletenessStateCode");
        }
    }
}
