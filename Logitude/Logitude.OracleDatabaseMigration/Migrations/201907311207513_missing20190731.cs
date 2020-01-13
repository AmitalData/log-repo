namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class missing20190731 : DbMigration
    {
        public override void Up()
        {
            //////RenameColumn(table: "dbo.ShipmentComputedFields", name: "ImporterDepositionRequestDet", newName: "ImporterDepositionRequestDetails");
            CreateTable(
                "dbo.CheckDigitControlAlgorithms",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 128),
                    Name = c.String(),
                    SearchFields = c.String(),
                })
                .PrimaryKey(t => t.Code);

            CreateTable(
                "dbo.NumberFormats",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 128),
                    Name = c.String(),
                    SearchFields = c.String(),
                })
                .PrimaryKey(t => t.Code);

            CreateTable(
                "dbo.INTTRABookingStatus",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 128),
                    Name = c.String(),
                    SearchFields = c.String(),
                })
                .PrimaryKey(t => t.Code);

            CreateTable(
                "dbo.INTTRABookingTransStatus",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 128),
                    Name = c.String(),
                    SearchFields = c.String(),
                })
                .PrimaryKey(t => t.Code);

            ////CreateTable(
            ////    "dbo.BIReportFolders",
            ////    c => new
            ////        {
            ////            Id = c.String(nullable: false, maxLength: 128),
            ////            Tenant = c.Int(nullable: false),
            ////            CreateDate = c.DateTime(nullable: false, precision: 7),
            ////            CreatedByUserId = c.String(maxLength: 15, unicode: false),
            ////            UpdateDate = c.DateTime(nullable: false, precision: 7),
            ////            UpdatedByUserId = c.String(maxLength: 15, unicode: false),
            ////            SearchFields = c.String(),
            ////            Name = c.String(),
            ////            Description = c.String(),
            ////            Index = c.Int(),
            ////        })
            ////    .PrimaryKey(t => t.Id)
            ////    .ForeignKey("dbo.Users", t => t.CreatedByUserId)
            ////    .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
            ////    .Index(t => t.CreatedByUserId)
            ////    .Index(t => t.UpdatedByUserId);

            CreateTable(
                "dbo.TMDayOffTypes",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 128),
                    Name = c.String(),
                    SearchFields = c.String(),
                })
                .PrimaryKey(t => t.Code);

            CreateTable(
                "dbo.ARInvoiceStocksStatus",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 2, unicode: false),
                    Name = c.String(nullable: false, maxLength: 20, unicode: false),
                    SearchFields = c.String(maxLength: 1000),
                })
                .PrimaryKey(t => t.Code);

            AddColumn("dbo.AccountingSettings", "IsARInvoiceChronologicalDates", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccountingSettings", "IsARPaymentChronologicalDates", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccountingSettings", "EnableInvoiceStocksManagement", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "CustomerTenantShareExportFile", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "NumberFormatCode", c => c.String(maxLength: 128));
            AddColumn("dbo.Tenants", "EcommerceSupportEmail", c => c.String(maxLength: 50, unicode: false));
            AddColumn("dbo.Tenants", "CBSA", c => c.String(maxLength: 5, unicode: false));
            AddColumn("dbo.Tenants", "CAAT", c => c.String(maxLength: 4, unicode: false));
            AddColumn("dbo.Tenants", "IsTestTenant", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "CheckDigitControlAlgorithmCode", c => c.String(nullable: true, maxLength: 128));
            AddColumn("dbo.Tenants", "ApplyVATForAllPartners", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "AdditionalPackagesOnly", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProductTypes", "RoutingRQuoteDefaultTemplateId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowHeaderLabelsPackages", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowHeaderLabelsContainers", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeContainers", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforePackages", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeQuoteHeaders", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeQuoteDetails", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeHeaders", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeFooters", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforePerContainers", c => c.Int(nullable: false));
            AddColumn("dbo.Countries", "IsNorthAmerica", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShippingLines", "CBSA", c => c.String(maxLength: 5, unicode: false));
            AddColumn("dbo.ShippingLines", "CAAT", c => c.String(maxLength: 4, unicode: false));
            AddColumn("dbo.States", "QBOTransactionLocationCode", c => c.String(maxLength: 3, unicode: false));
            AddColumn("dbo.ObjectTables", "IsTabsHidden", c => c.Boolean(nullable: false));
            AddColumn("dbo.Quotes", "ChargeableWeightInKG", c => c.Double());
            AddColumn("dbo.Quotes", "VolumeInCBM", c => c.Double());
            AddColumn("dbo.Quotes", "StartDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Quotes", "GrossWeightEdited", c => c.Boolean(nullable: false));
            AddColumn("dbo.Quotes", "ChargeableWeightEdited", c => c.Boolean(nullable: false));
            AddColumn("dbo.PackageTypes", "IsVehicle", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChargesTypes", "IsImport", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChargesTypes", "IsDomestic", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChargesTypes", "IsExport", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChargesTypes", "IsDrop", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChargesTypes", "ReceivablesDefaultCurrencyId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ChargesTypes", "PayablesDefaultCurrencyId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.APPayments", "AutomaticPaymentCheque", c => c.Boolean(nullable: false));
            AddColumn("dbo.ARInvoices", "ARInvoiceStockId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ARInvoices", "IsInvoiceNumberFromStock", c => c.Boolean(nullable: false));
            AddColumn("dbo.ARInvoices", "ConcurrencyGUID", c => c.String(nullable: false, maxLength: 40, unicode: false));
            AddColumn("dbo.ARPayments", "FechaPago", c => c.DateTime(precision: 7));
            AddColumn("dbo.ARPayments", "ApprovedDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.ARPayments", "ApprovedByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ARPayments", "FirstApproveDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.ARPayments", "IsFullAccounting", c => c.Boolean(nullable: false));
            AddColumn("dbo.ARPayments", "IsExternalEntity", c => c.Boolean(nullable: false));
            AddColumn("dbo.Automations", "Code", c => c.String(maxLength: 7, unicode: false));
            AddColumn("dbo.Shipments", "OperationalClosedByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Shipments", "AgentComputed", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Shipments", "OrderGrossWeightEdited", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shipments", "OrderChargeableWeightEdited", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shipments", "FirstAccountingCloseDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Shipments", "ComputedShipmentNumber", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Shipments", "INTTRABookingTransStatusCode", c => c.String(maxLength: 128));
            AddColumn("dbo.Shipments", "INTTRABookingStatusCode", c => c.String(maxLength: 128));
            AddColumn("dbo.Shipments", "ARInvoices", c => c.String(maxLength: 1000, unicode: false));
            AddColumn("dbo.ShipmentComputedFields", "Commodity", c => c.String(maxLength: 15));
            AddColumn("dbo.ShipmentComputedFields", "FirstPickupLocation", c => c.String(maxLength: 100));
            AddColumn("dbo.ShipmentComputedFields", "ContainersNumbers", c => c.String(maxLength: 1000));
            AddColumn("dbo.ShipmentComputedFields", "FirstPickupATD", c => c.DateTime(precision: 7));
            AddColumn("dbo.ShipmentComputedFields", "FirstPickupATA", c => c.DateTime(precision: 7));
            AddColumn("dbo.ShipmentComputedFields", "FinalDeliveryETD", c => c.DateTime(precision: 7));
            AddColumn("dbo.ShipmentComputedFields", "FinalDeliveryETA", c => c.DateTime(precision: 7));
            AddColumn("dbo.ShipmentComputedFields", "FinalDeliveryATD", c => c.DateTime(precision: 7));
            AddColumn("dbo.ShipmentComputedFields", "FinalDeliveryATA", c => c.DateTime(precision: 7));
            AddColumn("dbo.BankAccounts", "CurrencyId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AddColumn("dbo.GLAccounts", "CreatedByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.GLAccounts", "UpdatedByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.GLAccounts", "CreateDate", c => c.DateTime(nullable: false, precision: 7));
            AddColumn("dbo.GLAccounts", "UpdateDate", c => c.DateTime(nullable: false, precision: 7));
            AddColumn("dbo.BIReports", "BIReportFolderId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.FullAccountingSettings", "IsPaymentChequesActivated", c => c.Boolean(nullable: false));
            AddColumn("dbo.FullAccountingSettings", "GLAccounterCounterLength", c => c.Int());
            AddColumn("dbo.FullAccountingSettings", "PaymentChequesLogoId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.HybridTenantStates", "LastQueueDateTime", c => c.DateTime(precision: 7));
            AddColumn("dbo.InsideShipmentPackages", "Make", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.InsideShipmentPackages", "Model", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.InsideShipmentPackages", "Year", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.InsideShipmentPackages", "Color", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.InsideShipmentPackages", "ChassisNumber", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.InsideShipmentPackages", "RegistrationNumber", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.InsideShipmentPackages", "CountryId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ShipmentPackages", "Make", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPackages", "Model", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPackages", "Year", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPackages", "Color", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPackages", "ChassisNumber", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPackages", "RegistrationNumber", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPackages", "CountryId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Journals", "IsLedgerCreated", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProductTypeModifications", "RoutingRQuoteDefaultTemplateId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ReconcileExternalPageLines", "DebitAmount", c => c.Decimal(nullable: false, precision: 15, scale: 2));
            AddColumn("dbo.ReconcileExternalPageLines", "CreditAmount", c => c.Decimal(nullable: false, precision: 15, scale: 2));
            AddColumn("dbo.ReconciliationLines", "SearchFields", c => c.String());
            AddColumn("dbo.ReconciliationLines", "ReconciledWithTransactionId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ShipmentPickUpDeliveryPackages", "Make", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPickUpDeliveryPackages", "Model", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPickUpDeliveryPackages", "Year", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPickUpDeliveryPackages", "Color", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPickUpDeliveryPackages", "ChassisNumber", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPickUpDeliveryPackages", "RegistrationNumber", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentPickUpDeliveryPackages", "CountryId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TaskSchedulerHistory", "StartDateTimeUTC", c => c.DateTime(precision: 7));
            AddColumn("dbo.TaskSchedulerHistory", "EndDateTimeUTC", c => c.DateTime(precision: 7));
            AddColumn("dbo.TaskSchedulerHistory", "LogType", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.TaskSchedulerHistory", "LogFirstLine", c => c.String(maxLength: 1000, unicode: false));
            AddColumn("dbo.TasksScheduler", "ProcedureCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.TasksScheduler", "SchedulerDetailsXML", c => c.String());
            AddColumn("dbo.TasksScheduler", "Type", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TasksScheduler", "NextRunTimeUTC", c => c.DateTime(precision: 7));
            AddColumn("dbo.TasksScheduler", "StartDateTimeUTC", c => c.DateTime(precision: 7));
            AddColumn("dbo.TasksScheduler", "LastRunEndTime", c => c.DateTime(precision: 7));
            AddColumn("dbo.TasksScheduler", "LastRunEndTimeUTC", c => c.DateTime(precision: 7));
            AddColumn("dbo.TasksScheduler", "LastRunStartTimeUTC", c => c.DateTime(precision: 7));
            AddColumn("dbo.TasksScheduler", "LastRunStartTime", c => c.DateTime(precision: 7));
            AddColumn("dbo.TasksScheduler", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.TasksScheduler", "Status", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.TasksScheduler", "Retries", c => c.Int(nullable: false));
            AddColumn("dbo.TMProjects", "ExcludeFromProrating", c => c.Boolean(nullable: false));
            AddColumn("dbo.TMProjects", "DayOffTypeCode", c => c.String(maxLength: 128));
            AddColumn("dbo.TMProjects", "BlockedForDataEntry", c => c.Boolean(nullable: false));
            AddColumn("dbo.TMBudgets", "Inactive", c => c.Boolean(nullable: false));
            AddColumn("dbo.WarehouseEntries", "TotalVolumetricWeight", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            AddColumn("dbo.WarehouseEntryPackages", "Make", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseEntryPackages", "Model", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseEntryPackages", "Year", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseEntryPackages", "Color", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseEntryPackages", "ChassisNumber", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseEntryPackages", "RegistrationNumber", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseEntryPackages", "CountryId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseEntryPackages", "CommodityNumber", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.WarehouseReleasePackages", "Make", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseReleasePackages", "Model", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseReleasePackages", "Year", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseReleasePackages", "Color", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseReleasePackages", "ChassisNumber", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseReleasePackages", "RegistrationNumber", c => c.String(maxLength: 100));
            AddColumn("dbo.WarehouseReleasePackages", "CountryId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Tenants", "StockTypeCode", c => c.String(maxLength: 15, unicode: false));
            //AlterColumn("dbo.PackageTypes", "PrintAs", c => c.String(nullable: false, maxLength: 20, unicode: false));
            //AlterColumn("dbo.ChargesTypes", "Description", c => c.String(maxLength: 250));
            //AlterColumn("dbo.ARInvoices", "TotalVAT", c => c.Decimal(nullable: false, precision: 16, scale: 2));
            //AlterColumn("dbo.GLAccounts", "DisplayNumber", c => c.String(maxLength: 15, unicode: false));
            //AlterColumn("dbo.ProductTypeModifications", "QuotationDefaultTemplateId", c => c.String(maxLength: 15, unicode: false));
            //AlterColumn("dbo.ReconcileExternalPageLines", "Reference", c => c.String(maxLength: 30));
            //AlterColumn("dbo.TasksScheduler", "RepeatInMinutes", c => c.Int());
            //AlterColumn("dbo.TMEmployeeTimes", "Description", c => c.String(nullable: false, maxLength: 500));
            CreateIndex("dbo.Tenants", "NumberFormatCode");
            CreateIndex("dbo.Tenants", "CheckDigitControlAlgorithmCode");
            CreateIndex("dbo.ProductTypes", "RoutingRQuoteDefaultTemplateId");
            CreateIndex("dbo.ChargesTypes", "ReceivablesDefaultCurrencyId");
            CreateIndex("dbo.ChargesTypes", "PayablesDefaultCurrencyId");
            CreateIndex("dbo.ARPayments", "ApprovedByUserId");
            CreateIndex("dbo.Shipments", "OperationalClosedByUserId");
            CreateIndex("dbo.Shipments", "INTTRABookingTransStatusCode");
            CreateIndex("dbo.Shipments", "INTTRABookingStatusCode");
            CreateIndex("dbo.BankAccounts", "CurrencyId");
            CreateIndex("dbo.GLAccounts", "CreatedByUserId");
            CreateIndex("dbo.GLAccounts", "UpdatedByUserId");
            CreateIndex("dbo.BIReports", "BIReportFolderId");
            CreateIndex("dbo.InsideShipmentPackages", "CountryId");
            CreateIndex("dbo.ShipmentPackages", "CountryId");
            CreateIndex("dbo.ShipmentPickUpDeliveryPackages", "CountryId");
            CreateIndex("dbo.TMProjects", "DayOffTypeCode");
            CreateIndex("dbo.WarehouseEntryPackages", "CountryId");
            CreateIndex("dbo.WarehouseReleasePackages", "CountryId");
            AddForeignKey("dbo.ProductTypes", "RoutingRQuoteDefaultTemplateId", "dbo.QuoteTemplates", "Id");
            AddForeignKey("dbo.Tenants", "CheckDigitControlAlgorithmCode", "dbo.CheckDigitControlAlgorithms", "Code");
            AddForeignKey("dbo.Tenants", "NumberFormatCode", "dbo.NumberFormats", "Code");
            AddForeignKey("dbo.ChargesTypes", "PayablesDefaultCurrencyId", "dbo.Currencies", "Id");
            AddForeignKey("dbo.ChargesTypes", "ReceivablesDefaultCurrencyId", "dbo.Currencies", "Id");
            AddForeignKey("dbo.ARPayments", "ApprovedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Shipments", "INTTRABookingStatusCode", "dbo.INTTRABookingStatus", "Code");
            AddForeignKey("dbo.Shipments", "INTTRABookingTransStatusCode", "dbo.INTTRABookingTransStatus", "Code");
            AddForeignKey("dbo.Shipments", "OperationalClosedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.BankAccounts", "CurrencyId", "dbo.Currencies", "Id");
            AddForeignKey("dbo.GLAccounts", "CreatedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.GLAccounts", "UpdatedByUserId", "dbo.Users", "Id");
            // AddForeignKey("dbo.BIReports", "BIReportFolderId", "dbo.BIReportFolders", "Id");
            AddForeignKey("dbo.InsideShipmentPackages", "CountryId", "dbo.Countries", "Id");
            AddForeignKey("dbo.ShipmentPackages", "CountryId", "dbo.Countries", "Id");
            AddForeignKey("dbo.ShipmentPickUpDeliveryPackages", "CountryId", "dbo.Countries", "Id");
            AddForeignKey("dbo.TMProjects", "DayOffTypeCode", "dbo.TMDayOffTypes", "Code");
            AddForeignKey("dbo.WarehouseEntryPackages", "CountryId", "dbo.Countries", "Id");
            AddForeignKey("dbo.WarehouseReleasePackages", "CountryId", "dbo.Countries", "Id");
            DropColumn("dbo.AccountingSettings", "IsChronologicalDates");
            DropColumn("dbo.ReconcileExternalPageLines", "Amount");
            DropColumn("dbo.TasksScheduler", "LastRunTime");
            DropColumn("dbo.TasksScheduler", "ServiceClassName");
        }

        public override void Down()
        {
            AddColumn("dbo.TasksScheduler", "ServiceClassName", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.TasksScheduler", "LastRunTime", c => c.DateTime(precision: 7));
            AddColumn("dbo.ReconcileExternalPageLines", "Amount", c => c.Decimal(nullable: false, precision: 15, scale: 2));
            AddColumn("dbo.AccountingSettings", "IsChronologicalDates", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.WarehouseReleasePackages", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.WarehouseEntryPackages", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.TMProjects", "DayOffTypeCode", "dbo.TMDayOffTypes");
            DropForeignKey("dbo.ShipmentPickUpDeliveryPackages", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.ShipmentPackages", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.InsideShipmentPackages", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.BIReports", "BIReportFolderId", "dbo.BIReportFolders");
            DropForeignKey("dbo.BIReportFolders", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.BIReportFolders", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.GLAccounts", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.GLAccounts", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.BankAccounts", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Shipments", "OperationalClosedByUserId", "dbo.Users");
            DropForeignKey("dbo.Shipments", "INTTRABookingTransStatusCode", "dbo.INTTRABookingTransStatus");
            DropForeignKey("dbo.Shipments", "INTTRABookingStatusCode", "dbo.INTTRABookingStatus");
            DropForeignKey("dbo.ARPayments", "ApprovedByUserId", "dbo.Users");
            DropForeignKey("dbo.ChargesTypes", "ReceivablesDefaultCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ChargesTypes", "PayablesDefaultCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Tenants", "NumberFormatCode", "dbo.NumberFormats");
            DropForeignKey("dbo.Tenants", "CheckDigitControlAlgorithmCode", "dbo.CheckDigitControlAlgorithms");
            DropForeignKey("dbo.ProductTypes", "RoutingRQuoteDefaultTemplateId", "dbo.QuoteTemplates");
            DropIndex("dbo.WarehouseReleasePackages", new[] { "CountryId" });
            DropIndex("dbo.WarehouseEntryPackages", new[] { "CountryId" });
            DropIndex("dbo.TMProjects", new[] { "DayOffTypeCode" });
            DropIndex("dbo.ShipmentPickUpDeliveryPackages", new[] { "CountryId" });
            DropIndex("dbo.ShipmentPackages", new[] { "CountryId" });
            DropIndex("dbo.InsideShipmentPackages", new[] { "CountryId" });
            DropIndex("dbo.BIReportFolders", new[] { "UpdatedByUserId" });
            DropIndex("dbo.BIReportFolders", new[] { "CreatedByUserId" });
            DropIndex("dbo.BIReports", new[] { "BIReportFolderId" });
            DropIndex("dbo.GLAccounts", new[] { "UpdatedByUserId" });
            DropIndex("dbo.GLAccounts", new[] { "CreatedByUserId" });
            DropIndex("dbo.BankAccounts", new[] { "CurrencyId" });
            DropIndex("dbo.Shipments", new[] { "INTTRABookingStatusCode" });
            DropIndex("dbo.Shipments", new[] { "INTTRABookingTransStatusCode" });
            DropIndex("dbo.Shipments", new[] { "OperationalClosedByUserId" });
            DropIndex("dbo.ARPayments", new[] { "ApprovedByUserId" });
            DropIndex("dbo.ChargesTypes", new[] { "PayablesDefaultCurrencyId" });
            DropIndex("dbo.ChargesTypes", new[] { "ReceivablesDefaultCurrencyId" });
            DropIndex("dbo.ProductTypes", new[] { "RoutingRQuoteDefaultTemplateId" });
            DropIndex("dbo.Tenants", new[] { "CheckDigitControlAlgorithmCode" });
            DropIndex("dbo.Tenants", new[] { "NumberFormatCode" });
            AlterColumn("dbo.TMEmployeeTimes", "Description", c => c.String(maxLength: 500));
            AlterColumn("dbo.TasksScheduler", "RepeatInMinutes", c => c.Int(nullable: false));
            AlterColumn("dbo.ReconcileExternalPageLines", "Reference", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.ProductTypeModifications", "QuotationDefaultTemplateId", c => c.String());
            AlterColumn("dbo.GLAccounts", "DisplayNumber", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.ARInvoices", "TotalVAT", c => c.Decimal(precision: 16, scale: 2));
            AlterColumn("dbo.ChargesTypes", "Description", c => c.String(maxLength: 250, unicode: false));
            AlterColumn("dbo.PackageTypes", "PrintAs", c => c.String(maxLength: 5, unicode: false));
            AlterColumn("dbo.Tenants", "StockTypeCode", c => c.String());
            DropColumn("dbo.WarehouseReleasePackages", "CountryId");
            DropColumn("dbo.WarehouseReleasePackages", "RegistrationNumber");
            DropColumn("dbo.WarehouseReleasePackages", "ChassisNumber");
            DropColumn("dbo.WarehouseReleasePackages", "Color");
            DropColumn("dbo.WarehouseReleasePackages", "Year");
            DropColumn("dbo.WarehouseReleasePackages", "Model");
            DropColumn("dbo.WarehouseReleasePackages", "Make");
            DropColumn("dbo.WarehouseEntryPackages", "CommodityNumber");
            DropColumn("dbo.WarehouseEntryPackages", "CountryId");
            DropColumn("dbo.WarehouseEntryPackages", "RegistrationNumber");
            DropColumn("dbo.WarehouseEntryPackages", "ChassisNumber");
            DropColumn("dbo.WarehouseEntryPackages", "Color");
            DropColumn("dbo.WarehouseEntryPackages", "Year");
            DropColumn("dbo.WarehouseEntryPackages", "Model");
            DropColumn("dbo.WarehouseEntryPackages", "Make");
            DropColumn("dbo.WarehouseEntries", "TotalVolumetricWeight");
            DropColumn("dbo.TMBudgets", "Inactive");
            DropColumn("dbo.TMProjects", "BlockedForDataEntry");
            DropColumn("dbo.TMProjects", "DayOffTypeCode");
            DropColumn("dbo.TMProjects", "ExcludeFromProrating");
            DropColumn("dbo.TasksScheduler", "Retries");
            DropColumn("dbo.TasksScheduler", "Status");
            DropColumn("dbo.TasksScheduler", "Version");
            DropColumn("dbo.TasksScheduler", "LastRunStartTime");
            DropColumn("dbo.TasksScheduler", "LastRunStartTimeUTC");
            DropColumn("dbo.TasksScheduler", "LastRunEndTimeUTC");
            DropColumn("dbo.TasksScheduler", "LastRunEndTime");
            DropColumn("dbo.TasksScheduler", "StartDateTimeUTC");
            DropColumn("dbo.TasksScheduler", "NextRunTimeUTC");
            DropColumn("dbo.TasksScheduler", "Type");
            DropColumn("dbo.TasksScheduler", "SchedulerDetailsXML");
            DropColumn("dbo.TasksScheduler", "ProcedureCode");
            DropColumn("dbo.TaskSchedulerHistory", "LogFirstLine");
            DropColumn("dbo.TaskSchedulerHistory", "LogType");
            DropColumn("dbo.TaskSchedulerHistory", "EndDateTimeUTC");
            DropColumn("dbo.TaskSchedulerHistory", "StartDateTimeUTC");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "CountryId");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "RegistrationNumber");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "ChassisNumber");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "Color");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "Year");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "Model");
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "Make");
            DropColumn("dbo.ReconciliationLines", "ReconciledWithTransactionId");
            DropColumn("dbo.ReconciliationLines", "SearchFields");
            DropColumn("dbo.ReconcileExternalPageLines", "CreditAmount");
            DropColumn("dbo.ReconcileExternalPageLines", "DebitAmount");
            DropColumn("dbo.ProductTypeModifications", "RoutingRQuoteDefaultTemplateId");
            DropColumn("dbo.Journals", "IsLedgerCreated");
            DropColumn("dbo.ShipmentPackages", "CountryId");
            DropColumn("dbo.ShipmentPackages", "RegistrationNumber");
            DropColumn("dbo.ShipmentPackages", "ChassisNumber");
            DropColumn("dbo.ShipmentPackages", "Color");
            DropColumn("dbo.ShipmentPackages", "Year");
            DropColumn("dbo.ShipmentPackages", "Model");
            DropColumn("dbo.ShipmentPackages", "Make");
            DropColumn("dbo.InsideShipmentPackages", "CountryId");
            DropColumn("dbo.InsideShipmentPackages", "RegistrationNumber");
            DropColumn("dbo.InsideShipmentPackages", "ChassisNumber");
            DropColumn("dbo.InsideShipmentPackages", "Color");
            DropColumn("dbo.InsideShipmentPackages", "Year");
            DropColumn("dbo.InsideShipmentPackages", "Model");
            DropColumn("dbo.InsideShipmentPackages", "Make");
            DropColumn("dbo.HybridTenantStates", "LastQueueDateTime");
            DropColumn("dbo.FullAccountingSettings", "PaymentChequesLogoId");
            DropColumn("dbo.FullAccountingSettings", "GLAccounterCounterLength");
            DropColumn("dbo.FullAccountingSettings", "IsPaymentChequesActivated");
            DropColumn("dbo.BIReports", "BIReportFolderId");
            DropColumn("dbo.GLAccounts", "UpdateDate");
            DropColumn("dbo.GLAccounts", "CreateDate");
            DropColumn("dbo.GLAccounts", "UpdatedByUserId");
            DropColumn("dbo.GLAccounts", "CreatedByUserId");
            DropColumn("dbo.BankAccounts", "CurrencyId");
            DropColumn("dbo.ShipmentComputedFields", "FinalDeliveryATA");
            DropColumn("dbo.ShipmentComputedFields", "FinalDeliveryATD");
            DropColumn("dbo.ShipmentComputedFields", "FinalDeliveryETA");
            DropColumn("dbo.ShipmentComputedFields", "FinalDeliveryETD");
            DropColumn("dbo.ShipmentComputedFields", "FirstPickupATA");
            DropColumn("dbo.ShipmentComputedFields", "FirstPickupATD");
            DropColumn("dbo.ShipmentComputedFields", "ContainersNumbers");
            DropColumn("dbo.ShipmentComputedFields", "FirstPickupLocation");
            DropColumn("dbo.ShipmentComputedFields", "Commodity");
            DropColumn("dbo.Shipments", "ARInvoices");
            DropColumn("dbo.Shipments", "INTTRABookingStatusCode");
            DropColumn("dbo.Shipments", "INTTRABookingTransStatusCode");
            DropColumn("dbo.Shipments", "ComputedShipmentNumber");
            DropColumn("dbo.Shipments", "FirstAccountingCloseDate");
            DropColumn("dbo.Shipments", "OrderChargeableWeightEdited");
            DropColumn("dbo.Shipments", "OrderGrossWeightEdited");
            DropColumn("dbo.Shipments", "AgentComputed");
            DropColumn("dbo.Shipments", "OperationalClosedByUserId");
            DropColumn("dbo.Automations", "Code");
            DropColumn("dbo.ARPayments", "IsExternalEntity");
            DropColumn("dbo.ARPayments", "IsFullAccounting");
            DropColumn("dbo.ARPayments", "FirstApproveDate");
            DropColumn("dbo.ARPayments", "ApprovedByUserId");
            DropColumn("dbo.ARPayments", "ApprovedDate");
            DropColumn("dbo.ARPayments", "FechaPago");
            DropColumn("dbo.ARInvoices", "ConcurrencyGUID");
            DropColumn("dbo.ARInvoices", "IsInvoiceNumberFromStock");
            DropColumn("dbo.ARInvoices", "ARInvoiceStockId");
            DropColumn("dbo.APPayments", "AutomaticPaymentCheque");
            DropColumn("dbo.ChargesTypes", "PayablesDefaultCurrencyId");
            DropColumn("dbo.ChargesTypes", "ReceivablesDefaultCurrencyId");
            DropColumn("dbo.ChargesTypes", "IsDrop");
            DropColumn("dbo.ChargesTypes", "IsExport");
            DropColumn("dbo.ChargesTypes", "IsDomestic");
            DropColumn("dbo.ChargesTypes", "IsImport");
            DropColumn("dbo.PackageTypes", "IsVehicle");
            DropColumn("dbo.Quotes", "ChargeableWeightEdited");
            DropColumn("dbo.Quotes", "GrossWeightEdited");
            DropColumn("dbo.Quotes", "StartDate");
            DropColumn("dbo.Quotes", "VolumeInCBM");
            DropColumn("dbo.Quotes", "ChargeableWeightInKG");
            DropColumn("dbo.ObjectTables", "IsTabsHidden");
            DropColumn("dbo.States", "QBOTransactionLocationCode");
            DropColumn("dbo.ShippingLines", "CAAT");
            DropColumn("dbo.ShippingLines", "CBSA");
            DropColumn("dbo.Countries", "IsNorthAmerica");
            DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforePerContainers");
            DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeFooters");
            DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeHeaders");
            DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeQuoteDetails");
            DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeQuoteHeaders");
            DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforePackages");
            DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeContainers");
            DropColumn("dbo.QuoteTemplateSettings", "ShowHeaderLabelsContainers");
            DropColumn("dbo.QuoteTemplateSettings", "ShowHeaderLabelsPackages");
            DropColumn("dbo.ProductTypes", "RoutingRQuoteDefaultTemplateId");
            DropColumn("dbo.Users", "AdditionalPackagesOnly");
            DropColumn("dbo.Tenants", "ApplyVATForAllPartners");
            DropColumn("dbo.Tenants", "CheckDigitControlAlgorithmCode");
            DropColumn("dbo.Tenants", "IsTestTenant");
            DropColumn("dbo.Tenants", "CAAT");
            DropColumn("dbo.Tenants", "CBSA");
            DropColumn("dbo.Tenants", "EcommerceSupportEmail");
            DropColumn("dbo.Tenants", "NumberFormatCode");
            DropColumn("dbo.Tenants", "CustomerTenantShareExportFile");
            DropColumn("dbo.AccountingSettings", "EnableInvoiceStocksManagement");
            DropColumn("dbo.AccountingSettings", "IsARPaymentChronologicalDates");
            DropColumn("dbo.AccountingSettings", "IsARInvoiceChronologicalDates");
            DropTable("dbo.ARInvoiceStocksStatus");
            DropTable("dbo.TMDayOffTypes");
            DropTable("dbo.BIReportFolders");
            DropTable("dbo.INTTRABookingTransStatus");
            DropTable("dbo.INTTRABookingStatus");
            DropTable("dbo.NumberFormats");
            DropTable("dbo.CheckDigitControlAlgorithms");
            RenameColumn(table: "dbo.ShipmentComputedFields", name: "ImporterDepositionRequestDetails", newName: "ImporterDepositionRequestDet");
        }
    }
}
