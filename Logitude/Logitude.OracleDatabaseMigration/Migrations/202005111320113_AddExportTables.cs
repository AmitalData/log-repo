namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExportTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.AmountTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        EnglishName = c.String(maxLength: 100, unicode: false),
                        LocalName = c.String(maxLength: 100),
                        SearchFields = c.String(maxLength: 1000),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.ClaimReasonTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        EnglishName = c.String(maxLength: 100, unicode: false),
                        LocalName = c.String(maxLength: 100),
                        SearchFields = c.String(maxLength: 1000),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.ClassificationTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        EnglishName = c.String(maxLength: 100, unicode: false),
                        LocalName = c.String(maxLength: 100),
                        SearchFields = c.String(maxLength: 1000),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.PartyRelationshipTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        EnglishName = c.String(maxLength: 100, unicode: false),
                        LocalName = c.String(maxLength: 100),
                        SearchFields = c.String(maxLength: 1000),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.SuppInvoiceItemsAbachStatement",
                c => new
                    {
                        DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        InvoiceCounterKey = c.Int(nullable: false),
                        InvoiceItemLineNumber = c.Int(nullable: false),
                        SequenceNumeric = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        StatementType = c.String(maxLength: 3),
                        StatementInd = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => new { t.DeclarationId, t.InvoiceCounterKey, t.InvoiceItemLineNumber, t.SequenceNumeric })
                .ForeignKey("Customs.SupplierInvoiceItems", t => new { t.DeclarationId, t.InvoiceCounterKey, t.InvoiceItemLineNumber })
                .Index(t => new { t.DeclarationId, t.InvoiceCounterKey, t.InvoiceItemLineNumber });
            
            CreateTable(
                "Customs.TransactionNatureTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        EnglishName = c.String(maxLength: 100, unicode: false),
                        LocalName = c.String(maxLength: 100),
                        SearchFields = c.String(maxLength: 1000),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.SupplierInvoiceItemsPrices",
                c => new
                    {
                        DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        InvoiceCounterKey = c.Int(nullable: false),
                        InvoiceItemLineNumber = c.Int(nullable: false),
                        LineNumber = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        AdditionalPriceTypeCode = c.String(nullable: false, maxLength: 4, unicode: false),
                        AdditionalPrice = c.Decimal(nullable: false, precision: 16, scale: 2),
                    })
                .PrimaryKey(t => new { t.DeclarationId, t.InvoiceCounterKey, t.InvoiceItemLineNumber, t.LineNumber })
                .ForeignKey("Customs.AmountTypes", t => t.AdditionalPriceTypeCode)
                .ForeignKey("Customs.SupplierInvoiceItems", t => new { t.DeclarationId, t.InvoiceCounterKey, t.InvoiceItemLineNumber })
                .Index(t => new { t.DeclarationId, t.InvoiceCounterKey, t.InvoiceItemLineNumber })
                .Index(t => t.AdditionalPriceTypeCode);
            
            CreateTable(
                "Customs.SupplierInvoicePayments",
                c => new
                    {
                        DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        InvoiceCounterKey = c.Int(nullable: false),
                        SequenceNumeric = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        PaymentTypeCode = c.String(nullable: false, maxLength: 4, unicode: false),
                        PaymentAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                    })
                .PrimaryKey(t => new { t.DeclarationId, t.InvoiceCounterKey, t.SequenceNumeric })
                .ForeignKey("Customs.PaymentTypes", t => t.PaymentTypeCode)
                .ForeignKey("Customs.SupplierInvoices", t => new { t.DeclarationId, t.InvoiceCounterKey })
                .Index(t => new { t.DeclarationId, t.InvoiceCounterKey })
                .Index(t => t.PaymentTypeCode);
            
            CreateTable(
                "Customs.SupplierInvoiceUCRs",
                c => new
                    {
                        DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        InvoiceCounterKey = c.Int(nullable: false),
                        SequenceNumeric = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        SupplierChargeID = c.String(maxLength: 35),
                        AgentChargeID = c.String(maxLength: 35),
                    })
                .PrimaryKey(t => new { t.DeclarationId, t.InvoiceCounterKey, t.SequenceNumeric })
                .ForeignKey("Customs.SupplierInvoices", t => new { t.DeclarationId, t.InvoiceCounterKey })
                .Index(t => new { t.DeclarationId, t.InvoiceCounterKey });
            
            AddColumn("Customs.SupplierInvoiceItems", "ClassificationTypeCode", c => c.String(maxLength: 4, unicode: false));
            AddColumn("Customs.SupplierInvoiceItems", "TransactionNatureCode", c => c.String(maxLength: 4, unicode: false));
            AddColumn("Customs.SupplierInvoiceItems", "ClaimReasonCode", c => c.String(maxLength: 4, unicode: false));
            AddColumn("Customs.SupplierInvoices", "BuyerName", c => c.String(maxLength: 35));
            AddColumn("Customs.SupplierInvoices", "BuyerAddress", c => c.String(maxLength: 256));
            AddColumn("Customs.SupplierInvoices", "BuyerCountryCode", c => c.String(maxLength: 2, unicode: false));
            AddColumn("Customs.SupplierInvoices", "BuyerRoleCode", c => c.String(maxLength: 4, unicode: false));
            AddColumn("Customs.SupplierInvoices", "PartyRelationshipCode", c => c.String(maxLength: 4, unicode: false));
            AddColumn("Customs.SupplierInvoiceItemVehicles", "IdentifierID", c => c.String(maxLength: 35));
            AddColumn("Customs.SupplierInvoiceItemVehicles", "VehicleIDTypeCode", c => c.String(maxLength: 4, unicode: false));
            CreateIndex("Customs.SupplierInvoiceItems", "ClassificationTypeCode");
            CreateIndex("Customs.SupplierInvoiceItems", "TransactionNatureCode");
            CreateIndex("Customs.SupplierInvoiceItems", "ClaimReasonCode");
            CreateIndex("Customs.SupplierInvoices", "BuyerCountryCode");
            CreateIndex("Customs.SupplierInvoices", "BuyerRoleCode");
            CreateIndex("Customs.SupplierInvoices", "PartyRelationshipCode");
            CreateIndex("Customs.SupplierInvoiceItemVehicles", "VehicleIDTypeCode");
            AddForeignKey("Customs.SupplierInvoiceItems", "ClaimReasonCode", "Customs.ClaimReasonTypes", "Code");
            AddForeignKey("Customs.SupplierInvoiceItems", "ClassificationTypeCode", "Customs.ClassificationTypes", "Code");
            AddForeignKey("Customs.SupplierInvoices", "BuyerCountryCode", "Customs.CustomsCountries", "Code");
            AddForeignKey("Customs.SupplierInvoices", "BuyerRoleCode", "Customs.CustomerRoleTypes", "Code");
            AddForeignKey("Customs.SupplierInvoices", "PartyRelationshipCode", "Customs.PartyRelationshipTypes", "Code");
            AddForeignKey("Customs.SupplierInvoiceItems", "TransactionNatureCode", "Customs.TransactionNatureTypes", "Code");
            AddForeignKey("Customs.SupplierInvoiceItemVehicles", "VehicleIDTypeCode", "Customs.CargoIdentityQualifiers", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.SupplierInvoiceUCRs", new[] { "DeclarationId", "InvoiceCounterKey" }, "Customs.SupplierInvoices");
            DropForeignKey("Customs.SupplierInvoicePayments", new[] { "DeclarationId", "InvoiceCounterKey" }, "Customs.SupplierInvoices");
            DropForeignKey("Customs.SupplierInvoicePayments", "PaymentTypeCode", "Customs.PaymentTypes");
            DropForeignKey("Customs.SupplierInvoiceItemVehicles", "VehicleIDTypeCode", "Customs.CargoIdentityQualifiers");
            DropForeignKey("Customs.SupplierInvoiceItemsPrices", new[] { "DeclarationId", "InvoiceCounterKey", "InvoiceItemLineNumber" }, "Customs.SupplierInvoiceItems");
            DropForeignKey("Customs.SupplierInvoiceItemsPrices", "AdditionalPriceTypeCode", "Customs.AmountTypes");
            DropForeignKey("Customs.SuppInvoiceItemsAbachStatement", new[] { "DeclarationId", "InvoiceCounterKey", "InvoiceItemLineNumber" }, "Customs.SupplierInvoiceItems");
            DropForeignKey("Customs.SupplierInvoiceItems", "TransactionNatureCode", "Customs.TransactionNatureTypes");
            DropForeignKey("Customs.SupplierInvoices", "PartyRelationshipCode", "Customs.PartyRelationshipTypes");
            DropForeignKey("Customs.SupplierInvoices", "BuyerRoleCode", "Customs.CustomerRoleTypes");
            DropForeignKey("Customs.SupplierInvoices", "BuyerCountryCode", "Customs.CustomsCountries");
            DropForeignKey("Customs.SupplierInvoiceItems", "ClassificationTypeCode", "Customs.ClassificationTypes");
            DropForeignKey("Customs.SupplierInvoiceItems", "ClaimReasonCode", "Customs.ClaimReasonTypes");
            DropIndex("Customs.SupplierInvoiceUCRs", new[] { "DeclarationId", "InvoiceCounterKey" });
            DropIndex("Customs.SupplierInvoicePayments", new[] { "PaymentTypeCode" });
            DropIndex("Customs.SupplierInvoicePayments", new[] { "DeclarationId", "InvoiceCounterKey" });
            DropIndex("Customs.SupplierInvoiceItemVehicles", new[] { "VehicleIDTypeCode" });
            DropIndex("Customs.SupplierInvoiceItemsPrices", new[] { "AdditionalPriceTypeCode" });
            DropIndex("Customs.SupplierInvoiceItemsPrices", new[] { "DeclarationId", "InvoiceCounterKey", "InvoiceItemLineNumber" });
            DropIndex("Customs.SupplierInvoices", new[] { "PartyRelationshipCode" });
            DropIndex("Customs.SupplierInvoices", new[] { "BuyerRoleCode" });
            DropIndex("Customs.SupplierInvoices", new[] { "BuyerCountryCode" });
            DropIndex("Customs.SupplierInvoiceItems", new[] { "ClaimReasonCode" });
            DropIndex("Customs.SupplierInvoiceItems", new[] { "TransactionNatureCode" });
            DropIndex("Customs.SupplierInvoiceItems", new[] { "ClassificationTypeCode" });
            DropIndex("Customs.SuppInvoiceItemsAbachStatement", new[] { "DeclarationId", "InvoiceCounterKey", "InvoiceItemLineNumber" });
            DropColumn("Customs.SupplierInvoiceItemVehicles", "VehicleIDTypeCode");
            DropColumn("Customs.SupplierInvoiceItemVehicles", "IdentifierID");
            DropColumn("Customs.SupplierInvoices", "PartyRelationshipCode");
            DropColumn("Customs.SupplierInvoices", "BuyerRoleCode");
            DropColumn("Customs.SupplierInvoices", "BuyerCountryCode");
            DropColumn("Customs.SupplierInvoices", "BuyerAddress");
            DropColumn("Customs.SupplierInvoices", "BuyerName");
            DropColumn("Customs.SupplierInvoiceItems", "ClaimReasonCode");
            DropColumn("Customs.SupplierInvoiceItems", "TransactionNatureCode");
            DropColumn("Customs.SupplierInvoiceItems", "ClassificationTypeCode");
            DropTable("Customs.SupplierInvoiceUCRs");
            DropTable("Customs.SupplierInvoicePayments");
            DropTable("Customs.SupplierInvoiceItemsPrices");
            DropTable("Customs.TransactionNatureTypes");
            DropTable("Customs.SuppInvoiceItemsAbachStatement");
            DropTable("Customs.PartyRelationshipTypes");
            DropTable("Customs.ClassificationTypes");
            DropTable("Customs.ClaimReasonTypes");
            DropTable("Customs.AmountTypes");
        }
    }
}
