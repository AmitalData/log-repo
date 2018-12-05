namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingCustomsMM : DbMigration
    {
        public override void Up()
        {


            DropForeignKey("Customs.Declarations", "CourierDeclarationStatusCode", "Customs.CourierDeclarationStatuses");
            DropForeignKey("Customs.Declarations", "CourierManifestStatusCode", "Customs.CourierManifestStatuses");
            DropForeignKey("Customs.Declarations", "CourierPaymentStatusCode", "Customs.CourierPaymentStatuses");
            DropIndex("Customs.Declarations", new[] { "CourierManifestStatusCode" });
            DropIndex("Customs.Declarations", new[] { "CourierDeclarationStatusCode" });
            DropIndex("Customs.Declarations", new[] { "CourierPaymentStatusCode" });
            CreateTable(
                "Customs.ActionCodes",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 2, unicode: false),
                    LocalName = c.String(maxLength: 40),
                    EnglishName = c.String(maxLength: 40, unicode: false),
                    SearchFields = c.String(maxLength: 1000),
                    Inactive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Code);

            CreateTable(
                "Customs.CargoSplitRequestStatuses",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 1, unicode: false),
                    EnglishName = c.String(maxLength: 40, unicode: false),
                    SearchFields = c.String(maxLength: 1000),
                    LocalName = c.String(maxLength: 40),
                    Inactive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Code);

            CreateTable(
                "Customs.DeclarationCargoSplits",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 15, unicode: false),
                    Tenant = c.Int(nullable: false),
                    RequestDate = c.DateTime(nullable: false, precision: 7),
                    SearchFields = c.String(),
                    ActionTypeCode = c.String(maxLength: 2, unicode: false),
                    RequestReason = c.String(maxLength: 2, unicode: false),
                    RequestNumber = c.String(maxLength: 9, unicode: false),
                    RequestRemarks = c.String(maxLength: 255),
                    CargoTypeCode = c.String(maxLength: 3, unicode: false),
                    ManifestNumber = c.String(maxLength: 35, unicode: false),
                    SecondCargoID = c.String(maxLength: 35, unicode: false),
                    ThirdCargoID = c.String(maxLength: 35, unicode: false),
                    DeclarationId = c.String(maxLength: 15, unicode: false),
                    IsClosed = c.Boolean(nullable: false),
                    ResponseStatusCode = c.String(maxLength: 1, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Customs.ActionCodes", t => t.ActionTypeCode)
                .ForeignKey("Customs.CargoIdentifireTypes", t => t.CargoTypeCode)
                .ForeignKey("Customs.CargoSplitRequestStatuses", t => t.ResponseStatusCode)
                .ForeignKey("Customs.Declarations", t => t.DeclarationId)
                .ForeignKey("Customs.SplitOrMergeReasons", t => t.RequestReason)
                .Index(t => t.ActionTypeCode)
                .Index(t => t.RequestReason)
                .Index(t => t.CargoTypeCode)
                .Index(t => t.DeclarationId)
                .Index(t => t.ResponseStatusCode);

            CreateTable(
                "Customs.SplitOrMergeReasons",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 2, unicode: false),
                    LocalName = c.String(maxLength: 40),
                    EnglishName = c.String(maxLength: 40, unicode: false),
                    SearchFields = c.String(maxLength: 1000),
                    Inactive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Code);

            DropColumn("Customs.CustomsCountries", "testfield");
            DropColumn("Customs.Declarations", "CourierManifestStatusCode");
            DropColumn("Customs.Declarations", "CourierDeclarationStatusCode");
            DropColumn("Customs.Declarations", "CourierPaymentStatusCode");

            CreateTable(
                "Customs.DecCargoSplitCargoIdentifiers",
                c => new
                    {
                        DeclarationCargoSplitId = c.String(nullable: false, maxLength: 15, unicode: false),
                        LineNumber = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        CargoIdentifierKey1 = c.String(maxLength: 35, unicode: false),
                        CargoIdentifierKey2 = c.String(maxLength: 35, unicode: false),
                        CargoIdentifierKey3 = c.String(maxLength: 35, unicode: false),
                    })
                .PrimaryKey(t => new { t.DeclarationCargoSplitId, t.LineNumber })
                .ForeignKey("Customs.DeclarationCargoSplits", t => t.DeclarationCargoSplitId)
                .Index(t => t.DeclarationCargoSplitId);
            
            AddColumn("Customs.Vehicles", "PassportName", c => c.String(maxLength: 55));
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.DecCargoSplitCargoIdentifiers", "DeclarationCargoSplitId", "Customs.DeclarationCargoSplits");
            DropIndex("Customs.DecCargoSplitCargoIdentifiers", new[] { "DeclarationCargoSplitId" });
            DropColumn("Customs.Vehicles", "PassportName");
            DropTable("Customs.DecCargoSplitCargoIdentifiers");
        }
    }
}
