namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentChequeLocalNameMigration : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.INTTRABookingStatuses", newName: "PaymentGatewayPartners");
            //DropForeignKey("dbo.Quotes", "QuoteHTMLDocumentId", "dbo.Documents");
            //DropForeignKey("dbo.Shipments", "AgentComputed", "dbo.Cards");
            //DropForeignKey("dbo.Shipments", "INTTRABookingStatusCode", "dbo.INTTRABookingStatuses");
            //DropForeignKey("dbo.Shipments", "INTTRABookingTransStatusCode", "dbo.INTTRABookingTransStatuses");
            //DropForeignKey("dbo.BankAccounts", "CurrencyId", "dbo.Currencies");
            //DropForeignKey("dbo.BIReportsExecutionLogs", "StatusCode", "dbo.CommunicationStatusTypes");
            //DropForeignKey("dbo.BIReportsExecutionLogs", "CreatedByUserId", "dbo.Users");
            //DropForeignKey("dbo.CardContactProducts", "CardContactId", "dbo.CardContacts");
            //DropForeignKey("dbo.CardContactProducts", "ProductTypeCode", "dbo.ProductTypes");
            //DropForeignKey("dbo.JournalExternalReconciles", "JournalId", "dbo.Journals");
            //DropForeignKey("dbo.JournalExternalReconciles", "LedgerTransactionId", "dbo.LedgerTransactions");
            //DropForeignKey("dbo.JournalExternalReconciles", "ReconcileExternalPageLineId", "dbo.ReconcileExternalPageLines");
            //DropForeignKey("dbo.TaskSchedulerHistory", "LogDocumentId", "dbo.Documents");
            //DropForeignKey("dbo.TariffSurchargesUpdates", "CreatedByUserId", "dbo.Users");
            //DropForeignKey("dbo.TariffSurchargesUpdates", "UpdateMethodCode", "dbo.TariffSurchargesUpdateMethods");
            //DropForeignKey("dbo.TariffVersionAllInCharges", "AddedByUserId", "dbo.Users");
            //DropForeignKey("dbo.TariffVersionAllInCharges", "ChargesTypeId", "dbo.ChargesTypes");
            //DropIndex("dbo.Quotes", new[] { "QuoteHTMLDocumentId" });
            //DropIndex("dbo.Shipments", new[] { "AgentComputed" });
            //DropIndex("dbo.Shipments", new[] { "INTTRABookingTransStatusCode" });
            //DropIndex("dbo.Shipments", new[] { "INTTRABookingStatusCode" });
            //DropIndex("dbo.BankAccounts", new[] { "CurrencyId" });
            //DropIndex("dbo.BIReportsExecutionLogs", new[] { "CreatedByUserId" });
            //DropIndex("dbo.BIReportsExecutionLogs", new[] { "StatusCode" });
            //DropIndex("dbo.CardContactProducts", new[] { "CardContactId" });
            //DropIndex("dbo.CardContactProducts", new[] { "ProductTypeCode" });
            //DropIndex("dbo.JournalExternalReconciles", new[] { "JournalId" });
            //DropIndex("dbo.JournalExternalReconciles", new[] { "LedgerTransactionId" });
            //DropIndex("dbo.JournalExternalReconciles", new[] { "ReconcileExternalPageLineId" });
            //DropIndex("dbo.TaskSchedulerHistory", new[] { "LogDocumentId" });
            //DropIndex("dbo.TariffSurchargesUpdates", new[] { "CreatedByUserId" });
            //DropIndex("dbo.TariffSurchargesUpdates", new[] { "UpdateMethodCode" });
            //DropIndex("dbo.TariffVersionAllInCharges", new[] { "ChargesTypeId" });
            //DropIndex("dbo.TariffVersionAllInCharges", new[] { "AddedByUserId" });
            //DropPrimaryKey("dbo.PaymentGatewayPartners");
            //AddColumn("dbo.Shipments", "CutoffDate", c => c.DateTime());
            //AddColumn("dbo.TasksScheduler", "ServiceClassName", c => c.String(maxLength: 100, unicode: false));
            //AddColumn("dbo.Tariffs", "Description", c => c.String(maxLength: 250));
            //AlterColumn("dbo.PaymentGatewayPartners", "Code", c => c.String(nullable: false, maxLength: 128));
            //AlterColumn("dbo.PaymentGatewayPartners", "Name", c => c.String());
            //AlterColumn("dbo.PaymentGatewayPartners", "SearchFields", c => c.String());
            AlterColumn("dbo.PaymentCheques", "PayToName", c => c.String(nullable: false, maxLength: 100));
            //AlterColumn("dbo.Tariffs", "StartDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.Tariffs", "ExpirationDate", c => c.DateTime(nullable: false));
            //AddPrimaryKey("dbo.PaymentGatewayPartners", "Code");
            //AddForeignKey("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", "dbo.PaymentGatewayPartners", "Code");
            //DropColumn("dbo.AccountingIntegrityChecks", "ShouldFix");
            //DropColumn("dbo.Users", "AdditionalPackagesOnly");
            //DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeContainers");
            //DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforePackages");
            //DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeQuoteHeaders");
            //DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeQuoteDetails");
            //DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeHeaders");
            //DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeFooters");
            //DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforePerContainers");
            //DropColumn("dbo.QuoteTemplateSettings", "QuoteTemplatePDFMarginTop");
            //DropColumn("dbo.QuoteTemplateSettings", "QuoteTemplatePDFMarginBottom");
            //DropColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesPerContainers");
            //DropColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesPackages");
            //DropColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesContainers");
            //DropColumn("dbo.Customers", "LastOpportunitySubject");
            //DropColumn("dbo.Customers", "LastOpportunityStatus");
            //DropColumn("dbo.VatTypes", "RecognizedPercentage");
            //DropColumn("dbo.AccountingPaymentMethods", "LocalName");
            //DropColumn("dbo.Quotes", "StartDate");
            //DropColumn("dbo.Quotes", "QuoteHTMLDocumentId");
            //DropColumn("dbo.ChargesGroups", "ViewOrder");
            //DropColumn("dbo.ARInvoices", "DocumentFilingId");
            //DropColumn("dbo.Shipments", "INTTRABookingTransStatusCode");
            //DropColumn("dbo.Shipments", "INTTRABookingStatusCode");
            //DropColumn("dbo.ShipmentMasterDatas", "CutoffDate");
            //DropColumn("dbo.BankAccounts", "CurrencyId");
            //DropColumn("dbo.CustomsInterfaceSettings", "AMCAirStartDate");
            //DropColumn("dbo.CustomsInterfaceSettings", "AMCOceanStartDate");
            //DropColumn("dbo.LedgerTransactions", "InProgressExternalReconcile");
            //DropColumn("dbo.ReconcileExternalPageLines", "InProgressExternalReconcile");
            //DropColumn("dbo.FullAccountingSettings", "PaymentChequesLogoId");
            //DropColumn("dbo.QuoteSettings", "CopyExchangeRates");
            //DropColumn("dbo.TaskSchedulerHistory", "LogDocumentId");
            //DropColumn("dbo.TasksScheduler", "ProcedureCode");
            //DropColumn("dbo.TasksScheduler", "AverageRunTime");
            //DropColumn("dbo.ShipmentPayables", "TariffId");
            //DropColumn("dbo.ShipmentPayables", "TariffNumber");
            //DropColumn("dbo.ShipmentPayables", "TariffVersion");
            //DropColumn("dbo.TariffLines", "IsFromAllOtherPorts");
            //DropColumn("dbo.TariffLines", "IsToAllOtherPorts");
            //DropColumn("dbo.TariffLines", "Surcharge1MinPrice");
            //DropColumn("dbo.TariffLines", "Surcharge2MinPrice");
            //DropColumn("dbo.TariffLines", "Surcharge3MinPrice");
            //DropColumn("dbo.TariffLines", "Surcharge4MinPrice");
            //DropColumn("dbo.TariffLines", "Surcharge5MinPrice");
            //DropColumn("dbo.TariffLines", "Surcharge6MinPrice");
            //DropColumn("dbo.TariffLines", "Surcharge7MinPrice");
            //DropColumn("dbo.TariffLines", "Surcharge8MinPrice");
            //DropColumn("dbo.TariffLines", "Surcharge9MinPrice");
            //DropColumn("dbo.TariffLines", "Surcharge10MinPrice");
            //DropColumn("dbo.TariffLines", "CurrencyId");
            //DropColumn("dbo.Tariffs", "Notes");
            //DropColumn("dbo.TariffVersions", "InitialEnddate");
            //DropColumn("dbo.WarehouseEntries", "LastStatusUpdateDate");
            //DropColumn("dbo.WarehouseEntries", "ConnectedTo");
            //DropColumn("dbo.WarehouseEntries", "Ratio");
            //DropColumn("dbo.WarehouseReleases", "ConnectedTo");
            //DropTable("dbo.INTTRABookingTransStatuses");
            //DropTable("dbo.BIReportsExecutionLogs");
            //DropTable("dbo.CardContactProducts");
            //DropTable("dbo.JournalExternalReconciles");
            //DropTable("dbo.SchedulerProcedure");
            //DropTable("dbo.TariffSurchargesUpdateMethods");
            //DropTable("dbo.TariffSurchargesUpdates");
            //DropTable("dbo.TariffVersionAllInCharges");
            //DropTable("dbo.PaymentGatewayPartners");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PaymentGatewayPartners",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        SearchFields = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.TariffVersionAllInCharges",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                        ChargesTypeId = c.String(nullable: false, maxLength: 15, unicode: false),
                        AddedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        AddDate = c.DateTime(nullable: false),
                        TariffId = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TariffSurchargesUpdates",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        TariffId = c.String(maxLength: 15, unicode: false),
                        StartDate = c.DateTime(),
                        LinesUpdated = c.Int(),
                        From = c.String(),
                        To = c.String(),
                        Version = c.Int(nullable: false),
                        Surcharges = c.String(),
                        UpdateMethodCode = c.String(maxLength: 3, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TariffSurchargesUpdateMethods",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 3, unicode: false),
                        Name = c.String(maxLength: 40, unicode: false),
                        SearchFields = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.SchedulerProcedure",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 100, unicode: false),
                        Name = c.String(maxLength: 100, unicode: false),
                        SearchFields = c.String(maxLength: 1000, unicode: false),
                        Description = c.String(maxLength: 1000, unicode: false),
                        IsInternallyDefined = c.Boolean(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.JournalExternalReconciles",
                c => new
                    {
                        JournalId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Line = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        LedgerTransactionId = c.String(nullable: false, maxLength: 15, unicode: false),
                        ReconcileExternalPageLineId = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => new { t.JournalId, t.Line });
            
            CreateTable(
                "dbo.CardContactProducts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CardContactId = c.String(nullable: false, maxLength: 15, unicode: false),
                        ProductTypeCode = c.String(nullable: false, maxLength: 2, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BIReportsExecutionLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        StatusCode = c.String(maxLength: 4, unicode: false),
                        ExceptionMessage = c.String(),
                        DoneDate = c.DateTime(),
                        ReportFilterXML = c.String(),
                        BIReportId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.INTTRABookingTransStatuses",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        Name = c.String(nullable: false, maxLength: 40, unicode: false),
                        SearchFields = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Code);
            
            AddColumn("dbo.WarehouseReleases", "ConnectedTo", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.WarehouseEntries", "Ratio", c => c.Double());
            AddColumn("dbo.WarehouseEntries", "ConnectedTo", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.WarehouseEntries", "LastStatusUpdateDate", c => c.DateTime());
            AddColumn("dbo.TariffVersions", "InitialEnddate", c => c.DateTime());
            AddColumn("dbo.Tariffs", "Notes", c => c.String(maxLength: 250));
            AddColumn("dbo.TariffLines", "CurrencyId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TariffLines", "Surcharge10MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge9MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge8MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge7MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge6MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge5MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge4MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge3MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge2MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge1MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "IsToAllOtherPorts", c => c.Boolean(nullable: false));
            AddColumn("dbo.TariffLines", "IsFromAllOtherPorts", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShipmentPayables", "TariffVersion", c => c.Int(nullable: false));
            AddColumn("dbo.ShipmentPayables", "TariffNumber", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.ShipmentPayables", "TariffId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TasksScheduler", "AverageRunTime", c => c.Double(nullable: false));
            AddColumn("dbo.TasksScheduler", "ProcedureCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.TaskSchedulerHistory", "LogDocumentId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.QuoteSettings", "CopyExchangeRates", c => c.Boolean(nullable: false));
            AddColumn("dbo.FullAccountingSettings", "PaymentChequesLogoId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ReconcileExternalPageLines", "InProgressExternalReconcile", c => c.Boolean(nullable: false));
            AddColumn("dbo.LedgerTransactions", "InProgressExternalReconcile", c => c.Boolean(nullable: false));
            AddColumn("dbo.CustomsInterfaceSettings", "AMCOceanStartDate", c => c.DateTime());
            AddColumn("dbo.CustomsInterfaceSettings", "AMCAirStartDate", c => c.DateTime());
            AddColumn("dbo.BankAccounts", "CurrencyId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AddColumn("dbo.ShipmentMasterDatas", "CutoffDate", c => c.DateTime());
            AddColumn("dbo.Shipments", "INTTRABookingStatusCode", c => c.String(maxLength: 4, unicode: false));
            AddColumn("dbo.Shipments", "INTTRABookingTransStatusCode", c => c.String(maxLength: 4, unicode: false));
            AddColumn("dbo.ARInvoices", "DocumentFilingId", c => c.String());
            AddColumn("dbo.ChargesGroups", "ViewOrder", c => c.Int(nullable: false));
            AddColumn("dbo.Quotes", "QuoteHTMLDocumentId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Quotes", "StartDate", c => c.DateTime());
            AddColumn("dbo.AccountingPaymentMethods", "LocalName", c => c.String(maxLength: 100));
            AddColumn("dbo.VatTypes", "RecognizedPercentage", c => c.Double());
            AddColumn("dbo.Customers", "LastOpportunityStatus", c => c.String(maxLength: 60, unicode: false));
            AddColumn("dbo.Customers", "LastOpportunitySubject", c => c.String(maxLength: 250));
            AddColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesContainers", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesPackages", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesPerContainers", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "QuoteTemplatePDFMarginBottom", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "QuoteTemplatePDFMarginTop", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforePerContainers", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeFooters", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeHeaders", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeQuoteDetails", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeQuoteHeaders", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforePackages", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeContainers", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "AdditionalPackagesOnly", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccountingIntegrityChecks", "ShouldFix", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", "dbo.PaymentGatewayPartners");
            DropPrimaryKey("dbo.PaymentGatewayPartners");
            AlterColumn("dbo.Tariffs", "ExpirationDate", c => c.DateTime());
            AlterColumn("dbo.Tariffs", "StartDate", c => c.DateTime());
            AlterColumn("dbo.PaymentCheques", "PayToName", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.PaymentGatewayPartners", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("dbo.PaymentGatewayPartners", "Name", c => c.String(nullable: false, maxLength: 40, unicode: false));
            AlterColumn("dbo.PaymentGatewayPartners", "Code", c => c.String(nullable: false, maxLength: 4, unicode: false));
            DropColumn("dbo.Tariffs", "Description");
            DropColumn("dbo.TasksScheduler", "ServiceClassName");
            DropColumn("dbo.Shipments", "CutoffDate");
            AddPrimaryKey("dbo.PaymentGatewayPartners", "Code");
            CreateIndex("dbo.TariffVersionAllInCharges", "AddedByUserId");
            CreateIndex("dbo.TariffVersionAllInCharges", "ChargesTypeId");
            CreateIndex("dbo.TariffSurchargesUpdates", "UpdateMethodCode");
            CreateIndex("dbo.TariffSurchargesUpdates", "CreatedByUserId");
            CreateIndex("dbo.TaskSchedulerHistory", "LogDocumentId");
            CreateIndex("dbo.JournalExternalReconciles", "ReconcileExternalPageLineId");
            CreateIndex("dbo.JournalExternalReconciles", "LedgerTransactionId");
            CreateIndex("dbo.JournalExternalReconciles", "JournalId");
            CreateIndex("dbo.CardContactProducts", "ProductTypeCode");
            CreateIndex("dbo.CardContactProducts", "CardContactId");
            CreateIndex("dbo.BIReportsExecutionLogs", "StatusCode");
            CreateIndex("dbo.BIReportsExecutionLogs", "CreatedByUserId");
            CreateIndex("dbo.BankAccounts", "CurrencyId");
            CreateIndex("dbo.Shipments", "INTTRABookingStatusCode");
            CreateIndex("dbo.Shipments", "INTTRABookingTransStatusCode");
            CreateIndex("dbo.Shipments", "AgentComputed");
            CreateIndex("dbo.Quotes", "QuoteHTMLDocumentId");
            AddForeignKey("dbo.TariffVersionAllInCharges", "ChargesTypeId", "dbo.ChargesTypes", "Id");
            AddForeignKey("dbo.TariffVersionAllInCharges", "AddedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.TariffSurchargesUpdates", "UpdateMethodCode", "dbo.TariffSurchargesUpdateMethods", "Code");
            AddForeignKey("dbo.TariffSurchargesUpdates", "CreatedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.TaskSchedulerHistory", "LogDocumentId", "dbo.Documents", "Id");
            AddForeignKey("dbo.JournalExternalReconciles", "ReconcileExternalPageLineId", "dbo.ReconcileExternalPageLines", "Id");
            AddForeignKey("dbo.JournalExternalReconciles", "LedgerTransactionId", "dbo.LedgerTransactions", "Id");
            AddForeignKey("dbo.JournalExternalReconciles", "JournalId", "dbo.Journals", "Id");
            AddForeignKey("dbo.CardContactProducts", "ProductTypeCode", "dbo.ProductTypes", "Code");
            AddForeignKey("dbo.CardContactProducts", "CardContactId", "dbo.CardContacts", "Id");
            AddForeignKey("dbo.BIReportsExecutionLogs", "CreatedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.BIReportsExecutionLogs", "StatusCode", "dbo.CommunicationStatusTypes", "Code");
            AddForeignKey("dbo.BankAccounts", "CurrencyId", "dbo.Currencies", "Id");
            AddForeignKey("dbo.Shipments", "INTTRABookingTransStatusCode", "dbo.INTTRABookingTransStatuses", "Code");
            AddForeignKey("dbo.Shipments", "INTTRABookingStatusCode", "dbo.INTTRABookingStatuses", "Code");
            AddForeignKey("dbo.Shipments", "AgentComputed", "dbo.Cards", "Id");
            AddForeignKey("dbo.Quotes", "QuoteHTMLDocumentId", "dbo.Documents", "Id");
            RenameTable(name: "dbo.PaymentGatewayPartners", newName: "INTTRABookingStatuses");
        }
    }
}
