namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPackageHasContainerExcField_MM_Fix : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("Customs.CourierMasters", "AirlineId", "dbo.Airlines");
            //DropForeignKey("Customs.DeclarationCargoSplits", "ResponseStatusCode", "Customs.CargoSplitRequestStatuses");
            //DropIndex("Customs.DeclarationCargoSplits", new[] { "ResponseStatusCode" });
            //DropPrimaryKey("Customs.CargoSplitRequestStatuses");
            //CreateTable(
            //    "dbo.DecisionTypes",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 128),
            //            LocalName = c.String(),
            //            EnglishName = c.String(),
            //            Inactive = c.Boolean(nullable: false),
            //            SearchFields = c.String(),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            //CreateTable(
            //    "Customs.DecCargoSplitCargoIdentifiers",
            //    c => new
            //        {
            //            DeclarationCargoSplitId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            LineNumber = c.Int(nullable: false),
            //            Tenant = c.Int(nullable: false),
            //            CargoIdentifierKey1 = c.String(maxLength: 35, unicode: false),
            //            CargoIdentifierKey2 = c.String(maxLength: 35, unicode: false),
            //            CargoIdentifierKey3 = c.String(maxLength: 35, unicode: false),
            //        })
            //    .PrimaryKey(t => new { t.DeclarationCargoSplitId, t.LineNumber })
            //    .ForeignKey("Customs.DeclarationCargoSplits", t => t.DeclarationCargoSplitId)
            //    .Index(t => t.DeclarationCargoSplitId);
            
            //AddColumn("Customs.ClaimsRelatedEntities", "DecisionCode", c => c.String(maxLength: 128));
            //AddColumn("Customs.ClaimsRelatedEntities", "DecisionNote", c => c.String(maxLength: 256));
            //AddColumn("Customs.ClaimsRelatedEntities", "EilatVatRefoundDecision", c => c.String(maxLength: 256));
            //AddColumn("Customs.ClaimsRelatedEntities", "DepositingAmount", c => c.Decimal(precision: 16, scale: 2));
            //AddColumn("Customs.ClaimsRelatedEntities", "RefundAmount", c => c.Decimal(precision: 16, scale: 2));
            //AddColumn("Customs.Declarations", "CourierSuspentionCode", c => c.String(maxLength: 2, unicode: false));
            //AddColumn("Customs.CourierMasters", "WeightValueCode", c => c.String(maxLength: 3, unicode: false));
            AddColumn("dbo.ShipmentPackages", "HasContainerException", c => c.Boolean(nullable: false));
            //AddColumn("Customs.Vehicles", "PassportName", c => c.String(maxLength: 55));
            //AddColumn("Customs.CustomsAirlines", "ICAO", c => c.String(maxLength: 3, unicode: false));
            //AlterColumn("Customs.CargoSplitRequestStatuses", "Code", c => c.String(nullable: false, maxLength: 2, unicode: false));
            //AlterColumn("Customs.CustomsVendors", "VATNumber", c => c.String(maxLength: 25, unicode: false));
            //AlterColumn("Customs.DeclarationCargoSplits", "ResponseStatusCode", c => c.String(maxLength: 2, unicode: false));
            //AddPrimaryKey("Customs.CargoSplitRequestStatuses", "Code");
            //CreateIndex("Customs.ClaimsRelatedEntities", "DecisionCode");
            //CreateIndex("Customs.Declarations", "CourierSuspentionCode");
            //CreateIndex("Customs.CourierMasters", "WeightValueCode");
            //CreateIndex("Customs.DeclarationCargoSplits", "ResponseStatusCode");
            //AddForeignKey("Customs.ClaimsRelatedEntities", "DecisionCode", "dbo.DecisionTypes", "Code");
            //AddForeignKey("Customs.Declarations", "CourierSuspentionCode", "Customs.DeclarationStatusTypes", "Code");
            //AddForeignKey("Customs.CourierMasters", "AirlineId", "Customs.CustomsAirlines", "Id");
            //AddForeignKey("Customs.CourierMasters", "WeightValueCode", "Customs.FreightPaymentMethods", "Code");
            //AddForeignKey("Customs.DeclarationCargoSplits", "ResponseStatusCode", "Customs.CargoSplitRequestStatuses", "Code");
        }
        
        public override void Down()
        {
            //DropForeignKey("Customs.DeclarationCargoSplits", "ResponseStatusCode", "Customs.CargoSplitRequestStatuses");
            //DropForeignKey("Customs.DecCargoSplitCargoIdentifiers", "DeclarationCargoSplitId", "Customs.DeclarationCargoSplits");
            //DropForeignKey("Customs.CourierMasters", "WeightValueCode", "Customs.FreightPaymentMethods");
            //DropForeignKey("Customs.CourierMasters", "AirlineId", "Customs.CustomsAirlines");
            //DropForeignKey("Customs.Declarations", "CourierSuspentionCode", "Customs.DeclarationStatusTypes");
            //DropForeignKey("Customs.ClaimsRelatedEntities", "DecisionCode", "dbo.DecisionTypes");
            //DropIndex("Customs.DeclarationCargoSplits", new[] { "ResponseStatusCode" });
            //DropIndex("Customs.DecCargoSplitCargoIdentifiers", new[] { "DeclarationCargoSplitId" });
            //DropIndex("Customs.CourierMasters", new[] { "WeightValueCode" });
            //DropIndex("Customs.Declarations", new[] { "CourierSuspentionCode" });
            //DropIndex("Customs.ClaimsRelatedEntities", new[] { "DecisionCode" });
            //DropPrimaryKey("Customs.CargoSplitRequestStatuses");
            //AlterColumn("Customs.DeclarationCargoSplits", "ResponseStatusCode", c => c.String(maxLength: 1, unicode: false));
            //AlterColumn("Customs.CustomsVendors", "VATNumber", c => c.String(maxLength: 15, unicode: false));
            //AlterColumn("Customs.CargoSplitRequestStatuses", "Code", c => c.String(nullable: false, maxLength: 1, unicode: false));
            //DropColumn("Customs.CustomsAirlines", "ICAO");
            //DropColumn("Customs.Vehicles", "PassportName");
            DropColumn("dbo.ShipmentPackages", "HasContainerException");
            //DropColumn("Customs.CourierMasters", "WeightValueCode");
            //DropColumn("Customs.Declarations", "CourierSuspentionCode");
            //DropColumn("Customs.ClaimsRelatedEntities", "RefundAmount");
            //DropColumn("Customs.ClaimsRelatedEntities", "DepositingAmount");
            //DropColumn("Customs.ClaimsRelatedEntities", "EilatVatRefoundDecision");
            //DropColumn("Customs.ClaimsRelatedEntities", "DecisionNote");
            //DropColumn("Customs.ClaimsRelatedEntities", "DecisionCode");
            //DropTable("Customs.DecCargoSplitCargoIdentifiers");
            //DropTable("dbo.DecisionTypes");
            //AddPrimaryKey("Customs.CargoSplitRequestStatuses", "Code");
            //CreateIndex("Customs.DeclarationCargoSplits", "ResponseStatusCode");
            //AddForeignKey("Customs.DeclarationCargoSplits", "ResponseStatusCode", "Customs.CargoSplitRequestStatuses", "Code");
            //AddForeignKey("Customs.CourierMasters", "AirlineId", "dbo.Airlines", "Id");
        }
    }
}
