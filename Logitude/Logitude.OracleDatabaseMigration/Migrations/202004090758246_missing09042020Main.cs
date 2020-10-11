namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missing09042020Main : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tenants", "LogBoxAdminUserId", "dbo.Contacts");
            //DropForeignKey("dbo.ReconcileExternalPages", "BankAccountId", "dbo.BankAccounts");
            DropForeignKey("dbo.SchedulerLogs", "HistoryId", "dbo.TaskSchedulerHistory");
            //DropForeignKey("dbo.JournalLines", "ActionCode", "dbo.JournalActionTypes");
            DropIndex("dbo.Tenants", new[] { "PasswordPolicyCode" });
            DropIndex("dbo.Tenants", new[] { "LogBoxAdminUserId" });
            DropIndex("dbo.APInvoiceLines", new[] { "ForiegnCurrencyId" });
            DropIndex("dbo.APPayments", new[] { "AccountingPaymentMethodId" });
            DropIndex("dbo.APPayments", new[] { "ApprovedByUserId" });
            DropIndex("dbo.JournalLines", new[] { "ActionCode" });
            DropIndex("dbo.ReconcileExternalPages", new[] { "BankAccountId" });
            DropIndex("dbo.SchedulerLogs", new[] { "HistoryId" });
            DropPrimaryKey("dbo.CustomerCompetitorProducts");
            CreateTable(
                "dbo.AccountingPartners",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 15, unicode: false),
                    Tenant = c.Int(nullable: false),
                    PrimaryContactName = c.String(maxLength: 60, unicode: false),
                    PrimaryContactEmail = c.String(maxLength: 70, unicode: false),
                    PrimaryContactPhone = c.String(maxLength: 25, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cards", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.SharedLogsContactLastLogins",
                c => new
                {
                    ContactId = c.String(nullable: false, maxLength: 15, unicode: false),
                    CardId = c.String(nullable: false, maxLength: 15, unicode: false),
                    PartnerTypeId = c.String(nullable: false, maxLength: 2, unicode: false),
                    Via = c.String(nullable: false, maxLength: 20, unicode: false),
                    LoginDateTime = c.DateTime(precision: 7),
                    Tenant = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.ContactId, t.CardId, t.PartnerTypeId, t.Via })
                .ForeignKey("dbo.Contacts", t => t.ContactId)
                .Index(t => t.ContactId);

            CreateTable(
                "dbo.LogBoxTenantSettings",
                c => new
                {
                    Id = c.Int(nullable: false),
                    IsDocumentsArchive = c.Boolean(nullable: false),
                    CustomerTenantShareImportFile = c.Boolean(nullable: false),
                    LogBoxAdminUserId = c.String(maxLength: 15, unicode: false),
                    DocumentShareAsDefault = c.Boolean(nullable: false),
                    StockTypeCode = c.String(),
                    AutoArchiveOnInvoice = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.LogBoxAdminUserId)
                .ForeignKey("dbo.Tenants", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.LogBoxAdminUserId);

            CreateTable(
                "dbo.SupportMailboxes",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Tenant = c.Int(nullable: false),
                    CreateDate = c.DateTime(nullable: false, precision: 7),
                    CreatedByUserId = c.String(maxLength: 15, unicode: false),
                    UpdateDate = c.DateTime(nullable: false, precision: 7),
                    UpdatedByUserId = c.String(maxLength: 15, unicode: false),
                    Mailbox = c.String(),
                    Inactive = c.Boolean(nullable: false),
                    IsDefault = c.Boolean(nullable: false),
                    SearchFields = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);

            CreateTable(
                "dbo.CustomsTransferHeaders",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 15, unicode: false),
                    Tenant = c.Int(nullable: false),
                    TransferNumber = c.String(nullable: false, maxLength: 20, unicode: false),
                    TransferDate = c.DateTime(precision: 7),
                    FileName = c.String(nullable: false, maxLength: 40, unicode: false),
                    CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                    CustomsTransferTypeCode = c.String(nullable: false, maxLength: 4, unicode: false),
                    SearchFields = c.String(maxLength: 1000),
                    Notes = c.String(maxLength: 250),
                    ShipmentNumber = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.CustomsTransferTypes", t => t.CustomsTransferTypeCode)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.CustomsTransferTypeCode);

            CreateTable(
                "dbo.CustomsTransferTypes",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 4, unicode: false),
                    Name = c.String(nullable: false, maxLength: 40, unicode: false),
                    SearchFields = c.String(maxLength: 1000),
                })
                .PrimaryKey(t => t.Code);

            CreateTable(
                "dbo.CustomsTransferLines",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 15, unicode: false),
                    Tenant = c.Int(nullable: false),
                    CustomsTransferHeaderId = c.String(nullable: false, maxLength: 15, unicode: false),
                    ShipmentId = c.String(nullable: false, maxLength: 15, unicode: false),
                    ShipmentNumber = c.String(maxLength: 20, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomsTransferHeaders", t => t.CustomsTransferHeaderId)
                .Index(t => t.CustomsTransferHeaderId);

            AddColumn("dbo.AccountingSettings", "RefreshToken", c => c.String(maxLength: 2000));
            AddColumn("dbo.AccountingSettings", "QBOOAuth", c => c.Int(nullable: false));
            AddColumn("dbo.AccountingSettings", "AllowManualARPaymentNumber", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccountingSettings", "AllowRegionalTaxManagement", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "HideFCLAllIn", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "AllowCustomersInAgentsLOV", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Addresses", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Cards", "CreatedByPartner", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.Cards", "StorageFreeDays", c => c.Int());
            AddColumn("dbo.Cards", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Users", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Branches", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Contacts", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            //AddColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_ContactId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_CardId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_PartnerTypeId", c => c.String(maxLength: 2, unicode: false));
            //AddColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_Via", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.Departments", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.QuoteTemplateSettings", "QuoteTemplatePDFMarginTop", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "QuoteTemplatePDFMarginBottom", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargPerContainers", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesPackages", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesContainers", c => c.Boolean(nullable: false));
            AddColumn("dbo.Customers", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Customers", "LastOpportunitySubject", c => c.String(maxLength: 250));
            AddColumn("dbo.Customers", "LastOpportunityStatus", c => c.String(maxLength: 60, unicode: false));
            AddColumn("dbo.Ranks", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Currencies", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Countries", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.PartnerTypes", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.PaymentTerms", "Code", c => c.String(maxLength: 4, unicode: false));
            AddColumn("dbo.ShippingLines", "INTTRAUpdatesShipment", c => c.Boolean(nullable: false));
            AddColumn("dbo.VatTypes", "PayablesExternalId", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.VatTypes", "ReceivablesExternalId", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.VatTypes", "RecognizedPercentage", c => c.Double());
            AddColumn("dbo.VatTypes", "IsRegionalTax", c => c.Boolean(nullable: false));
            AddColumn("dbo.States", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.ObjectTables", "HeaderScreenCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectTables", "DescriptionTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectTables", "NewButtonTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Tips", "ShortTextCodeCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AddColumn("dbo.Quotes", "QuoteHTMLDocumentId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Quotes", "Field11", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field12", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field13", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field14", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field15", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field16", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field17", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field18", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field19", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field20", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "CountryForStatisticsId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Quotes", "RequestDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Quotes", "EstimatedProfitInLocal", c => c.Double());
            AddColumn("dbo.Quotes", "EstimatedProfitInProfit", c => c.Double());
            AddColumn("dbo.Quotes", "ProfitCurrencyId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Quotes", "ProfitExchangeRate", c => c.Double());
            AddColumn("dbo.Directions", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Ports", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Incoterms", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.TransportModes", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.ShipmentTypes", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Tickets", "SupportMailboxId", c => c.String(maxLength: 128));
            AddColumn("dbo.Tickets", "LastCorrespondence", c => c.String(maxLength: 2000));
            AddColumn("dbo.AdvancedQueryFilters", "QueryCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.AdvancedQueryFilters", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ObjectFields", "FieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ObjectFields", "FullNameTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectFields", "HelpTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectFields", "ListTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectFields", "ShortNameTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectFields", "DisplayInAutomationAsEnitity", c => c.Boolean(nullable: false));
            AddColumn("dbo.ObjectFields", "RecordType", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Queries", "UniqueCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Queries", "OriginalQueryCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Queries", "NameTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Queries", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.Features", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.Features", "NameTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentLevels", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.AirlineMessagingRules", "RuleFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.APInvoices", "CreatedByPartner", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.APInvoiceLines", "ContainerTypeId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.APInvoiceLines", "Quantity", c => c.Int());
            AddColumn("dbo.ChargesTypes", "ApplyRegionalTax", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChargesGroups", "ViewOrder", c => c.Int(nullable: false));
            AddColumn("dbo.APPayments", "VendorBankAddress", c => c.String(maxLength: 100));
            AddColumn("dbo.APPayments", "VendorBankName", c => c.String(maxLength: 40));
            AddColumn("dbo.APPayments", "VendorBankAccountNumber", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.APPayments", "VendorSwift", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.APPayments", "VendorIBANNumber", c => c.String(maxLength: 30));
            AddColumn("dbo.APPayments", "AccountingCancelationDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.APPayments", "DontIncludeInDeductionReport", c => c.Boolean(nullable: false));
            AddColumn("dbo.APPayments", "CancelationNotes", c => c.String());
            AddColumn("dbo.APPayments", "Field1", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field2", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field3", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field4", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field5", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field6", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field7", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field8", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field9", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field10", c => c.String(maxLength: 250));
            AddColumn("dbo.AccountingPaymentMethods", "LocalName", c => c.String(maxLength: 100));
            AddColumn("dbo.ARInvoices", "DateForInterest", c => c.DateTime(precision: 7));
            AddColumn("dbo.ARInvoices", "DocumentFilingId", c => c.String(maxLength: 40, unicode: false));
            AddColumn("dbo.ARInvoices", "CreatedByPartner", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.ARInvoices", "BillToGLAccountId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ARPayments", "OpenAmountInLocalCurrency", c => c.Double());
            AddColumn("dbo.ARPayments", "CreatedByPartner", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.ARPayments", "IsPaymentNumberManuallySet", c => c.Boolean(nullable: false));
            AddColumn("dbo.ARPayments", "Field1", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field2", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field3", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field4", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field5", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field6", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field7", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field8", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field9", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field10", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "GrossWeightPerStorageDays", c => c.Double());
            AddColumn("dbo.Shipments", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Shipments", "INTTRABookingError", c => c.String(maxLength: 256, unicode: false));
            AddColumn("dbo.Shipments", "INTTRALastBookingResponse", c => c.String());
            AddColumn("dbo.Shipments", "NotInvoicedReceivablesAmount", c => c.Double());
            AddColumn("dbo.Shipments", "CreatedByPartner", c => c.String());
            AddColumn("dbo.Shipments", "FirstARInvoiceApprovalDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.Shipments", "WarehouseStorageFreeDays", c => c.Int());
            AddColumn("dbo.EntityStatus", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.ShipmentAdditionalCloudDatas", "IsUserIDNumberRequired", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumberUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumberXMLData", c => c.String());
            AddColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumber", c => c.String());
            AddColumn("dbo.ShipmentComputedFields", "OperationallyClosedByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ShipmentComputedFields", "NumberOfDeliveries", c => c.Int());
            AddColumn("dbo.ShipmentComputedFields", "LastPickupETA", c => c.DateTime(precision: 7));
            AddColumn("dbo.ShipmentComputedFields", "LastPickupETD", c => c.DateTime(precision: 7));
            AddColumn("dbo.ShipmentComputedFields", "LastPickupATA", c => c.DateTime(precision: 7));
            AddColumn("dbo.ShipmentComputedFields", "LastPickupATD", c => c.DateTime(precision: 7));
            AddColumn("dbo.ShipmentComputedFields", "DeliveryToPortId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ShipmentComputedFields", "DeliveryFrom", c => c.String(maxLength: 40, unicode: false));
            AddColumn("dbo.ShipmentComputedFields", "DeliveryTo", c => c.String(maxLength: 40, unicode: false));
            AddColumn("dbo.ShipmentComputedFields", "PickupFrom", c => c.String(maxLength: 40, unicode: false));
            AddColumn("dbo.ShipmentComputedFields", "PickupTo", c => c.String(maxLength: 40, unicode: false));
            AddColumn("dbo.ShipmentComputedFields", "OperationallyClosedByUserName", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentMasterDatas", "AutomaticLastUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.ShipmentMasterDatas", "CutoffDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.BankAccounts", "PrintingBranchNumber", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.BankAccounts", "PrintingAccountNumber", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.GLAccounts", "AllowEditChequePayToName", c => c.Boolean(nullable: false));
            AddColumn("dbo.GLAccounts", "ActiveForInterest", c => c.Boolean());
            AddColumn("dbo.GLAccounts", "InterestCalculationStartDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.GLAccounts", "ActiveForInterestCreditInvoice", c => c.Boolean());
            AddColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling", c => c.Int());
            AddColumn("dbo.GLAccounts", "InterestCreditLimit", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.GLAccounts", "NameForPrintingCheques", c => c.String(maxLength: 1000));
            AddColumn("dbo.GLAccounts", "Smallcashbook", c => c.Boolean(nullable: false));
            AddColumn("dbo.BIReports", "LastRunDate", c => c.DateTime(nullable: false, precision: 7));
            AddColumn("dbo.BIReports", "LastRunByUserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AddColumn("dbo.CreditLimitSettings", "CustomersShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "AgentsShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "ShipperConsigneeShipmentBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "CustomsAgentsShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "ShippingAgentsShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "AirlinesShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "ShippingLinesShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "TruckersShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "VendorsShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "WarehousesShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "CustomersInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "AgentsInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "ShipperConsigneeInvoiceBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "CustomsAgentsInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "ShippingAgentsInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "AirlinesInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "ShippingLinesInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "TruckersInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "VendorsInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "WarehousesInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CustomerFieldsUpdateSettings", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.CustomsInterfaceSettings", "AMCAirStartDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.CustomsInterfaceSettings", "AMCOceanStartDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.DocumentTypeTemplates", "BCC", c => c.String(maxLength: 500));
            AddColumn("dbo.FullAccountingSettings", "NumberOfAgingMonths", c => c.Int());
            AddColumn("dbo.JournalLines", "ActionId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.LedgerTransactions", "InProgressExternalReconcile", c => c.Boolean(nullable: false));
            AddColumn("dbo.MenuButtons", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.MenuButtons", "LabelTextCodeCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AddColumn("dbo.MenusTables", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.ObjectFieldModifications", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ObjectFieldValidations", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ObjectTableHelperControls", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.ObjectTableRuleFields", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ObjectTableRules", "TriggerFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ObjectTableTabs", "TabNameTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectTableTabs", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.PackageFeatures", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.QueryColumns", "QueryCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.QueryColumns", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ReconcileExternalPageLines", "InProgressExternalReconcile", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReconcileExternalPageLines", "InReconcileProgress", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReconcileExternalPages", "ObjectTableId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ReconcileExternalPages", "EntityId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ReportExecutionLogs", "RetryNumber", c => c.Int(nullable: false));
            AddColumn("dbo.ReportExecutionLogs", "StartDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.ReportExecutionLogs", "ExecutedByServerName", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Reports", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.Reports", "AvailableForScheduling", c => c.Boolean(nullable: false));
            AddColumn("dbo.Restrictions", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.RoleFeatures", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.RuleConditionFields", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.TaskSchedulerHistory", "LogDocumentId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TasksScheduler", "AverageRunTime", c => c.Double(nullable: false));
            AddColumn("dbo.TasksScheduler", "EntityId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.SchedulerProcedure", "IsInternallyDefined", c => c.Boolean());
            AddColumn("dbo.ScreenFields", "ScreenCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ScreenFields", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ScreenModifications", "ScreenCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.SharedLogisticsSettings", "IsShowAmountLocalCurrency", c => c.Boolean(nullable: false));
            AddColumn("dbo.SharedUserQueries", "QueryCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ShipmentPayables", "TariffId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ShipmentPayables", "TariffNumber", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.ShipmentPayables", "TariffVersion", c => c.Int(nullable: false));
            AddColumn("dbo.SLAHeaders", "SearchFields", c => c.String(maxLength: 1000));
            AddColumn("dbo.TariffSettings", "AirDefaultStepsId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TariffSettings", "LCLDefaultStepsId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Translations", "TextCodeCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AddColumn("dbo.WarehouseEntries", "LastStatusUpdateDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.WarehouseEntries", "ConnectedTo", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.WarehouseEntries", "Ratio", c => c.Double());
            AddColumn("dbo.WarehouseEntries", "ToTypeCode", c => c.String(maxLength: 4, unicode: false));
            AddColumn("dbo.WarehouseEntries", "FromTypeCode", c => c.String(maxLength: 4, unicode: false));
            AddColumn("dbo.WarehouseEntries", "FromCountryId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseEntries", "ToCountryId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "ConnectedTo", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.WarehouseReleases", "FromPortId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "ToPortId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "CustomerAddressId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "TotalVolumetricWeight", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            AddColumn("dbo.WarehouseReleases", "Ratio", c => c.Double());
            AddColumn("dbo.WarehouseReleases", "ToTypeCode", c => c.String(maxLength: 4, unicode: false));
            AddColumn("dbo.WarehouseReleases", "ToPartnerCardId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "ToAddressId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "ToAddressZipCode", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "ToAddressCity", c => c.String(maxLength: 25));
            AddColumn("dbo.WarehouseReleases", "ToAddressCountryId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "IsUsed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Tenants", "PasswordPolicyCode", c => c.String(maxLength: 4, unicode: false));
            AlterColumn("dbo.Cards", "BankName", c => c.String(maxLength: 40));
            AlterColumn("dbo.Cards", "BankAddress", c => c.String(maxLength: 100));
            //AlterColumn("dbo.Contacts", "ExternalId", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Industries", "Code", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AlterColumn("dbo.Quotes", "ExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.Features", "Code", c => c.String(maxLength: 120, unicode: false));
            AlterColumn("dbo.APInvoices", "InvoiceDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.APInvoices", "DueDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.APInvoices", "InvoiceCurrencyExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoices", "SubTotalInLocalCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoices", "SubTotalInInvoiceCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoices", "AmountInInvoiceCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoices", "AmountInLocalCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoices", "CreateDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.APInvoices", "ProfitCurrencyExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoices", "AmountInProfitCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoices", "UpdateDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.APInvoiceLines", "InvoiceCurrencyAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoiceLines", "LocalCurrencyAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoiceLines", "ProfitCurrencyAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoiceLines", "ForiegnCurrencyId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.APInvoicePayments", "LocalAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoicePayments", "ForeignAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.APPayments", "CreateDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.APPayments", "AmountInLocalCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.APPayments", "AccountingPaymentMethodId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.APPayments", "AmountInPaymentCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.APPayments", "PaymentCurrencyExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.APPayments", "RegisterDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.APPayments", "OpenAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.APPayments", "ValueDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.APPayments", "UpdateDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.APPayments", "ApprovedByUserId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.APInvoiceTotalVATs", "ProfitCurrencyVATAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoiceTotalVATs", "ProfitVatableAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoices", "InvoiceDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.ARInvoices", "DueDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.ARInvoices", "SubTotalInLocalCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoices", "SubTotalInInvoiceCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoices", "AmountInLocalCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoices", "AmountInInvoiceCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoices", "InvoiceCurrencyExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoices", "CreateDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.ARInvoices", "AmountDue", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoices", "AmountInProfitCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoices", "ProfitCurrencyExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoices", "UpdateDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.ARInvoiceLines", "ForiegnCurrencyAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceLines", "LocalCurrencyAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceLines", "InvoiceCurrencyAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceLines", "ForiegnExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceLines", "ProfitCurrencyAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoicePayments", "LocalAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoicePayments", "ForeignAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARPayments", "CreateDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.ARPayments", "AmountInLocalCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.ARPayments", "AmountInPaymentCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.ARPayments", "PaymentCurrencyExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.ARPayments", "RegisterDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.ARPayments", "OpenAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARPayments", "UpdateDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.ARInvoiceTotalVATs", "InvoiceCurrencyVATAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceTotalVATs", "LocalVATAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceTotalVATs", "InvoiceCurrencyVatableAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceTotalVATs", "LocalVatableAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceTotalVATs", "VatPercent", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceTotalVATs", "ProfitCurrencyVATAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceTotalVATs", "ProfitVatableAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.Shipments", "OpenReceivablesInLocalCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.Shipments", "AccountedReceivablesInLocal", c => c.Double(nullable: false));
            AlterColumn("dbo.Shipments", "SecurityKey", c => c.String(maxLength: 40, unicode: false));
            AlterColumn("dbo.GLAccounts", "DisplayNumber", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.Competitors", "Name", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.Followers", "CancelledDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.InsideShipmentPackages", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.ShipmentPackages", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.ShipmentPackages", "Temperature", c => c.String(maxLength: 8, unicode: false));
            AlterColumn("dbo.QuoteCharges", "CostExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.QuotePriceSteps", "Step", c => c.Double(nullable: false));
            AlterColumn("dbo.ShipmentOrderPackages", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.ShipmentPackageItems", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.ShipmentPayables", "UpdateDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.ShipmentPayables", "CreateDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.ShipmentPickUpDeliveryPackages", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.ShipmentReceivables", "UpdateDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.ShipmentReceivables", "CreateDate", c => c.DateTime(nullable: false, precision: 7));
            AlterColumn("dbo.TarrifCharges", "MaxPrice", c => c.Decimal(precision: 14, scale: 3));
            AlterColumn("dbo.TarrifCharges", "MinPrice", c => c.Decimal(precision: 14, scale: 3));
            AlterColumn("dbo.TarrifCharges", "UnitPrice", c => c.Decimal(precision: 14, scale: 3));
            AlterColumn("dbo.TarrifSteps", "Step", c => c.Decimal(precision: 14, scale: 3));
            AlterColumn("dbo.TarrifSteps", "MaxPrice", c => c.Decimal(precision: 14, scale: 3));
            AlterColumn("dbo.TarrifSteps", "MinPrice", c => c.Decimal(precision: 14, scale: 3));
            AlterColumn("dbo.TarrifSteps", "UnitPrice", c => c.Decimal(precision: 14, scale: 3));
            AlterColumn("dbo.VatTypePercentages", "Percentage", c => c.Double(nullable: false));
            AddPrimaryKey("dbo.CustomerCompetitorProducts", new[] { "CustomerId", "CompetitorId", "ProductTypeCode" });
            // CreateIndex("dbo.Contacts", new[] { "SharedLogisticsContactLastLogin_ContactId", "SharedLogisticsContactLastLogin_CardId", "SharedLogisticsContactLastLogin_PartnerTypeId", "SharedLogisticsContactLastLogin_Via" });
            CreateIndex("dbo.Tenants", "ChargeableWeightUnitCode");
            CreateIndex("dbo.Tenants", "PasswordPolicyCode");
            CreateIndex("dbo.Quotes", "QuoteHTMLDocumentId");
            CreateIndex("dbo.Quotes", "CountryForStatisticsId");
            CreateIndex("dbo.Quotes", "ProfitCurrencyId");
            CreateIndex("dbo.Tickets", "SupportMailboxId");
            CreateIndex("dbo.ObjectFields", "FieldCode", unique: true);
            CreateIndex("dbo.Features", "FeatureUniqeCode", unique: true);
            CreateIndex("dbo.APInvoiceLines", "ForiegnCurrencyId");
            CreateIndex("dbo.APInvoiceLines", "ContainerTypeId");
            CreateIndex("dbo.APPayments", "AccountingPaymentMethodId");
            CreateIndex("dbo.APPayments", "ApprovedByUserId");
            CreateIndex("dbo.Shipments", "AgentComputed");
            CreateIndex("dbo.ShipmentComputedFields", "OperationallyClosedByUserId");
            CreateIndex("dbo.ShipmentComputedFields", "DeliveryToPortId");
            CreateIndex("dbo.BIReports", "LastRunByUserId");
            CreateIndex("dbo.JournalLines", "ActionId");
            CreateIndex("dbo.TaskSchedulerHistory", "LogDocumentId");
            CreateIndex("dbo.WarehouseEntries", "FromCountryId");
            CreateIndex("dbo.WarehouseEntries", "ToCountryId");
            CreateIndex("dbo.WarehouseReleases", "FromPortId");
            CreateIndex("dbo.WarehouseReleases", "ToPortId");
            CreateIndex("dbo.WarehouseReleases", "ToPartnerCardId");
            CreateIndex("dbo.WarehouseReleases", "ToAddressId");
            CreateIndex("dbo.WarehouseReleases", "ToAddressCountryId");
            //AddForeignKey("dbo.Contacts", new[] { "SharedLogisticsContactLastLogin_ContactId", "SharedLogisticsContactLastLogin_CardId", "SharedLogisticsContactLastLogin_PartnerTypeId", "SharedLogisticsContactLastLogin_Via" }, "dbo.SharedLogsContactLastLogins", new[] { "ContactId", "CardId", "PartnerTypeId", "Via" });
            AddForeignKey("dbo.Tenants", "ChargeableWeightUnitCode", "dbo.WeightUnits", "Code");
            AddForeignKey("dbo.Quotes", "CountryForStatisticsId", "dbo.Countries", "Id");
            AddForeignKey("dbo.Quotes", "ProfitCurrencyId", "dbo.Currencies", "Id");
            AddForeignKey("dbo.Quotes", "QuoteHTMLDocumentId", "dbo.Documents", "Id");
            AddForeignKey("dbo.Tickets", "SupportMailboxId", "dbo.SupportMailboxes", "Id");
            AddForeignKey("dbo.APInvoiceLines", "ContainerTypeId", "dbo.PackageTypes", "Id");
            AddForeignKey("dbo.Shipments", "AgentComputed", "dbo.Cards", "Id");
            AddForeignKey("dbo.ShipmentComputedFields", "DeliveryToPortId", "dbo.Ports", "Id");
            AddForeignKey("dbo.ShipmentComputedFields", "OperationallyClosedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.BIReports", "LastRunByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.TaskSchedulerHistory", "LogDocumentId", "dbo.Documents", "Id");
            AddForeignKey("dbo.WarehouseEntries", "FromCountryId", "dbo.Countries", "Id");
            AddForeignKey("dbo.WarehouseEntries", "ToCountryId", "dbo.Countries", "Id");
            AddForeignKey("dbo.WarehouseReleases", "FromPortId", "dbo.Warehouses", "Id");
            AddForeignKey("dbo.WarehouseReleases", "ToAddressId", "dbo.Addresses", "Id");
            AddForeignKey("dbo.WarehouseReleases", "ToAddressCountryId", "dbo.Countries", "Id");
            AddForeignKey("dbo.WarehouseReleases", "ToPartnerCardId", "dbo.Cards", "Id");
            AddForeignKey("dbo.WarehouseReleases", "ToPortId", "dbo.Ports", "Id");
            AddForeignKey("dbo.JournalLines", "ActionId", "dbo.JournalActionTypes", "Id");
            DropColumn("dbo.Tenants", "IsDocumentsArchive");
            DropColumn("dbo.Tenants", "CustomerTenantShareImportFile");
            DropColumn("dbo.Tenants", "LogBoxAdminUserId");
            DropColumn("dbo.Tenants", "DocumentShareAsDefault");
            DropColumn("dbo.Tenants", "StockTypeCode");
            DropColumn("dbo.Tenants", "AutoArchiveOnInvoice");
            DropColumn("dbo.ARInvoices", "DateForVATInterest");
            DropColumn("dbo.Shipments", "CutoffDate");
            DropColumn("dbo.ReconcileExternalPages", "BankAccountId");
            DropTable("dbo.SchedulerLogs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SchedulerLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false, precision: 7),
                        Log = c.String(unicode: false),
                        HistoryId = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ReconcileExternalPages", "BankAccountId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AddColumn("dbo.Shipments", "CutoffDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.ARInvoices", "DateForVATInterest", c => c.DateTime(precision: 7));
            AddColumn("dbo.Tenants", "AutoArchiveOnInvoice", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "StockTypeCode", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tenants", "DocumentShareAsDefault", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "LogBoxAdminUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tenants", "CustomerTenantShareImportFile", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "IsDocumentsArchive", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.JournalLines", "ActionId", "dbo.JournalActionTypes");
            DropForeignKey("dbo.CustomsTransferLines", "CustomsTransferHeaderId", "dbo.CustomsTransferHeaders");
            DropForeignKey("dbo.CustomsTransferHeaders", "CustomsTransferTypeCode", "dbo.CustomsTransferTypes");
            DropForeignKey("dbo.CustomsTransferHeaders", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.WarehouseReleases", "ToPortId", "dbo.Ports");
            DropForeignKey("dbo.WarehouseReleases", "ToPartnerCardId", "dbo.Cards");
            DropForeignKey("dbo.WarehouseReleases", "ToAddressCountryId", "dbo.Countries");
            DropForeignKey("dbo.WarehouseReleases", "ToAddressId", "dbo.Addresses");
            DropForeignKey("dbo.WarehouseReleases", "FromPortId", "dbo.Warehouses");
            DropForeignKey("dbo.WarehouseEntries", "ToCountryId", "dbo.Countries");
            DropForeignKey("dbo.WarehouseEntries", "FromCountryId", "dbo.Countries");
            DropForeignKey("dbo.TaskSchedulerHistory", "LogDocumentId", "dbo.Documents");
            DropForeignKey("dbo.BIReports", "LastRunByUserId", "dbo.Users");
            DropForeignKey("dbo.ShipmentComputedFields", "OperationallyClosedByUserId", "dbo.Users");
            DropForeignKey("dbo.ShipmentComputedFields", "DeliveryToPortId", "dbo.Ports");
            DropForeignKey("dbo.Shipments", "AgentComputed", "dbo.Cards");
            DropForeignKey("dbo.APInvoiceLines", "ContainerTypeId", "dbo.PackageTypes");
            DropForeignKey("dbo.Tickets", "SupportMailboxId", "dbo.SupportMailboxes");
            DropForeignKey("dbo.SupportMailboxes", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.SupportMailboxes", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Quotes", "QuoteHTMLDocumentId", "dbo.Documents");
            DropForeignKey("dbo.Quotes", "ProfitCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Quotes", "CountryForStatisticsId", "dbo.Countries");
            DropForeignKey("dbo.AccountingPartners", "Id", "dbo.Cards");
            DropForeignKey("dbo.Tenants", "ChargeableWeightUnitCode", "dbo.WeightUnits");
            DropForeignKey("dbo.LogBoxTenantSettings", "Id", "dbo.Tenants");
            DropForeignKey("dbo.LogBoxTenantSettings", "LogBoxAdminUserId", "dbo.Contacts");
            DropForeignKey("dbo.Contacts", new[] { "SharedLogisticsContactLastLogin_ContactId", "SharedLogisticsContactLastLogin_CardId", "SharedLogisticsContactLastLogin_PartnerTypeId", "SharedLogisticsContactLastLogin_Via" }, "dbo.SharedLogisticsContactLastLogins");
            DropForeignKey("dbo.SharedLogisticsContactLastLogins", "ContactId", "dbo.Contacts");
            DropIndex("dbo.CustomsTransferLines", new[] { "CustomsTransferHeaderId" });
            DropIndex("dbo.CustomsTransferHeaders", new[] { "CustomsTransferTypeCode" });
            DropIndex("dbo.CustomsTransferHeaders", new[] { "CreatedByUserId" });
            DropIndex("dbo.WarehouseReleases", new[] { "ToAddressCountryId" });
            DropIndex("dbo.WarehouseReleases", new[] { "ToAddressId" });
            DropIndex("dbo.WarehouseReleases", new[] { "ToPartnerCardId" });
            DropIndex("dbo.WarehouseReleases", new[] { "ToPortId" });
            DropIndex("dbo.WarehouseReleases", new[] { "FromPortId" });
            DropIndex("dbo.WarehouseEntries", new[] { "ToCountryId" });
            DropIndex("dbo.WarehouseEntries", new[] { "FromCountryId" });
            DropIndex("dbo.TaskSchedulerHistory", new[] { "LogDocumentId" });
            DropIndex("dbo.JournalLines", new[] { "ActionId" });
            DropIndex("dbo.BIReports", new[] { "LastRunByUserId" });
            DropIndex("dbo.ShipmentComputedFields", new[] { "DeliveryToPortId" });
            DropIndex("dbo.ShipmentComputedFields", new[] { "OperationallyClosedByUserId" });
            DropIndex("dbo.Shipments", new[] { "AgentComputed" });
            DropIndex("dbo.APPayments", new[] { "ApprovedByUserId" });
            DropIndex("dbo.APPayments", new[] { "AccountingPaymentMethodId" });
            DropIndex("dbo.APInvoiceLines", new[] { "ContainerTypeId" });
            DropIndex("dbo.APInvoiceLines", new[] { "ForiegnCurrencyId" });
            DropIndex("dbo.Features", new[] { "FeatureUniqeCode" });
            DropIndex("dbo.ObjectFields", new[] { "FieldCode" });
            DropIndex("dbo.SupportMailboxes", new[] { "UpdatedByUserId" });
            DropIndex("dbo.SupportMailboxes", new[] { "CreatedByUserId" });
            DropIndex("dbo.Tickets", new[] { "SupportMailboxId" });
            DropIndex("dbo.Quotes", new[] { "ProfitCurrencyId" });
            DropIndex("dbo.Quotes", new[] { "CountryForStatisticsId" });
            DropIndex("dbo.Quotes", new[] { "QuoteHTMLDocumentId" });
            DropIndex("dbo.LogBoxTenantSettings", new[] { "LogBoxAdminUserId" });
            DropIndex("dbo.LogBoxTenantSettings", new[] { "Id" });
            DropIndex("dbo.Tenants", new[] { "PasswordPolicyCode" });
            DropIndex("dbo.Tenants", new[] { "ChargeableWeightUnitCode" });
            DropIndex("dbo.SharedLogisticsContactLastLogins", new[] { "ContactId" });
            DropIndex("dbo.Contacts", new[] { "SharedLogisticsContactLastLogin_ContactId", "SharedLogisticsContactLastLogin_CardId", "SharedLogisticsContactLastLogin_PartnerTypeId", "SharedLogisticsContactLastLogin_Via" });
            DropIndex("dbo.AccountingPartners", new[] { "Id" });
            DropPrimaryKey("dbo.CustomerCompetitorProducts");
            AlterColumn("dbo.VatTypePercentages", "Percentage", c => c.Double());
            AlterColumn("dbo.TarrifSteps", "UnitPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TarrifSteps", "MinPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TarrifSteps", "MaxPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TarrifSteps", "Step", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TarrifCharges", "UnitPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TarrifCharges", "MinPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TarrifCharges", "MaxPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ShipmentReceivables", "CreateDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.ShipmentReceivables", "UpdateDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.ShipmentPickUpDeliveryPackages", "Quantity", c => c.Int());
            AlterColumn("dbo.ShipmentPayables", "CreateDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.ShipmentPayables", "UpdateDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.ShipmentPackageItems", "Quantity", c => c.Int());
            AlterColumn("dbo.ShipmentOrderPackages", "Quantity", c => c.Int());
            AlterColumn("dbo.QuotePriceSteps", "Step", c => c.Double());
            AlterColumn("dbo.QuoteCharges", "CostExchangeRate", c => c.Double());
            AlterColumn("dbo.ShipmentPackages", "Temperature", c => c.Double());
            AlterColumn("dbo.ShipmentPackages", "Quantity", c => c.Int());
            AlterColumn("dbo.InsideShipmentPackages", "Quantity", c => c.Int());
            AlterColumn("dbo.Followers", "CancelledDate", c => c.Boolean());
            AlterColumn("dbo.Competitors", "Name", c => c.String(maxLength: 60));
            AlterColumn("dbo.GLAccounts", "DisplayNumber", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Shipments", "SecurityKey", c => c.String(maxLength: 40));
            AlterColumn("dbo.Shipments", "AccountedReceivablesInLocal", c => c.Double());
            AlterColumn("dbo.Shipments", "OpenReceivablesInLocalCurrency", c => c.Double());
            AlterColumn("dbo.ARInvoiceTotalVATs", "ProfitVatableAmount", c => c.Double());
            AlterColumn("dbo.ARInvoiceTotalVATs", "ProfitCurrencyVATAmount", c => c.Double());
            AlterColumn("dbo.ARInvoiceTotalVATs", "VatPercent", c => c.Double());
            AlterColumn("dbo.ARInvoiceTotalVATs", "LocalVatableAmount", c => c.Double());
            AlterColumn("dbo.ARInvoiceTotalVATs", "InvoiceCurrencyVatableAmount", c => c.Double());
            AlterColumn("dbo.ARInvoiceTotalVATs", "LocalVATAmount", c => c.Double());
            AlterColumn("dbo.ARInvoiceTotalVATs", "InvoiceCurrencyVATAmount", c => c.Double());
            AlterColumn("dbo.ARPayments", "UpdateDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.ARPayments", "OpenAmount", c => c.Double());
            AlterColumn("dbo.ARPayments", "RegisterDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.ARPayments", "PaymentCurrencyExchangeRate", c => c.Double());
            AlterColumn("dbo.ARPayments", "AmountInPaymentCurrency", c => c.Double());
            AlterColumn("dbo.ARPayments", "AmountInLocalCurrency", c => c.Double());
            AlterColumn("dbo.ARPayments", "CreateDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.ARInvoicePayments", "ForeignAmount", c => c.Double());
            AlterColumn("dbo.ARInvoicePayments", "LocalAmount", c => c.Double());
            AlterColumn("dbo.ARInvoiceLines", "ProfitCurrencyAmount", c => c.Double());
            AlterColumn("dbo.ARInvoiceLines", "ForiegnExchangeRate", c => c.Double());
            AlterColumn("dbo.ARInvoiceLines", "InvoiceCurrencyAmount", c => c.Double());
            AlterColumn("dbo.ARInvoiceLines", "LocalCurrencyAmount", c => c.Double());
            AlterColumn("dbo.ARInvoiceLines", "ForiegnCurrencyAmount", c => c.Double());
            AlterColumn("dbo.ARInvoices", "UpdateDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.ARInvoices", "ProfitCurrencyExchangeRate", c => c.Double());
            AlterColumn("dbo.ARInvoices", "AmountInProfitCurrency", c => c.Double());
            AlterColumn("dbo.ARInvoices", "AmountDue", c => c.Double());
            AlterColumn("dbo.ARInvoices", "CreateDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.ARInvoices", "InvoiceCurrencyExchangeRate", c => c.Double());
            AlterColumn("dbo.ARInvoices", "AmountInInvoiceCurrency", c => c.Double());
            AlterColumn("dbo.ARInvoices", "AmountInLocalCurrency", c => c.Double());
            AlterColumn("dbo.ARInvoices", "SubTotalInInvoiceCurrency", c => c.Double());
            AlterColumn("dbo.ARInvoices", "SubTotalInLocalCurrency", c => c.Double());
            AlterColumn("dbo.ARInvoices", "DueDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.ARInvoices", "InvoiceDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.APInvoiceTotalVATs", "ProfitVatableAmount", c => c.Double());
            AlterColumn("dbo.APInvoiceTotalVATs", "ProfitCurrencyVATAmount", c => c.Double());
            AlterColumn("dbo.APPayments", "ApprovedByUserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.APPayments", "UpdateDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.APPayments", "ValueDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.APPayments", "OpenAmount", c => c.Double());
            AlterColumn("dbo.APPayments", "RegisterDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.APPayments", "PaymentCurrencyExchangeRate", c => c.Double());
            AlterColumn("dbo.APPayments", "AmountInPaymentCurrency", c => c.Double());
            AlterColumn("dbo.APPayments", "AccountingPaymentMethodId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.APPayments", "AmountInLocalCurrency", c => c.Double());
            AlterColumn("dbo.APPayments", "CreateDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.APInvoicePayments", "ForeignAmount", c => c.Double());
            AlterColumn("dbo.APInvoicePayments", "LocalAmount", c => c.Double());
            AlterColumn("dbo.APInvoiceLines", "ForiegnCurrencyId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.APInvoiceLines", "ProfitCurrencyAmount", c => c.Double());
            AlterColumn("dbo.APInvoiceLines", "LocalCurrencyAmount", c => c.Double());
            AlterColumn("dbo.APInvoiceLines", "InvoiceCurrencyAmount", c => c.Double());
            AlterColumn("dbo.APInvoices", "UpdateDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.APInvoices", "AmountInProfitCurrency", c => c.Double());
            AlterColumn("dbo.APInvoices", "ProfitCurrencyExchangeRate", c => c.Double());
            AlterColumn("dbo.APInvoices", "CreateDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.APInvoices", "AmountInLocalCurrency", c => c.Double());
            AlterColumn("dbo.APInvoices", "AmountInInvoiceCurrency", c => c.Double());
            AlterColumn("dbo.APInvoices", "SubTotalInInvoiceCurrency", c => c.Double());
            AlterColumn("dbo.APInvoices", "SubTotalInLocalCurrency", c => c.Double());
            AlterColumn("dbo.APInvoices", "InvoiceCurrencyExchangeRate", c => c.Double());
            AlterColumn("dbo.APInvoices", "DueDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.APInvoices", "InvoiceDate", c => c.DateTime(precision: 7));
            AlterColumn("dbo.Features", "Code", c => c.String(nullable: false, maxLength: 80, unicode: false));
            AlterColumn("dbo.Quotes", "ExchangeRate", c => c.Double());
            AlterColumn("dbo.Industries", "Code", c => c.String());
            AlterColumn("dbo.Contacts", "ExternalId", c => c.String());
            AlterColumn("dbo.Cards", "BankAddress", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.Cards", "BankName", c => c.String(maxLength: 40, unicode: false));
            AlterColumn("dbo.Tenants", "PasswordPolicyCode", c => c.String(nullable: false, maxLength: 4, unicode: false));
            DropColumn("dbo.WarehouseReleases", "IsUsed");
            DropColumn("dbo.WarehouseReleases", "ToAddressCountryId");
            DropColumn("dbo.WarehouseReleases", "ToAddressCity");
            DropColumn("dbo.WarehouseReleases", "ToAddressZipCode");
            DropColumn("dbo.WarehouseReleases", "ToAddressId");
            DropColumn("dbo.WarehouseReleases", "ToPartnerCardId");
            DropColumn("dbo.WarehouseReleases", "ToTypeCode");
            DropColumn("dbo.WarehouseReleases", "Ratio");
            DropColumn("dbo.WarehouseReleases", "TotalVolumetricWeight");
            DropColumn("dbo.WarehouseReleases", "CustomerAddressId");
            DropColumn("dbo.WarehouseReleases", "ToPortId");
            DropColumn("dbo.WarehouseReleases", "FromPortId");
            DropColumn("dbo.WarehouseReleases", "ConnectedTo");
            DropColumn("dbo.WarehouseEntries", "ToCountryId");
            DropColumn("dbo.WarehouseEntries", "FromCountryId");
            DropColumn("dbo.WarehouseEntries", "FromTypeCode");
            DropColumn("dbo.WarehouseEntries", "ToTypeCode");
            DropColumn("dbo.WarehouseEntries", "Ratio");
            DropColumn("dbo.WarehouseEntries", "ConnectedTo");
            DropColumn("dbo.WarehouseEntries", "LastStatusUpdateDate");
            DropColumn("dbo.Translations", "TextCodeCode");
            DropColumn("dbo.TariffSettings", "LCLDefaultStepsId");
            DropColumn("dbo.TariffSettings", "AirDefaultStepsId");
            DropColumn("dbo.SLAHeaders", "SearchFields");
            DropColumn("dbo.ShipmentPayables", "TariffVersion");
            DropColumn("dbo.ShipmentPayables", "TariffNumber");
            DropColumn("dbo.ShipmentPayables", "TariffId");
            DropColumn("dbo.SharedUserQueries", "QueryCode");
            DropColumn("dbo.SharedLogisticsSettings", "IsShowAmountLocalCurrency");
            DropColumn("dbo.ScreenModifications", "ScreenCode");
            DropColumn("dbo.ScreenFields", "ObjectFieldCode");
            DropColumn("dbo.ScreenFields", "ScreenCode");
            DropColumn("dbo.SchedulerProcedure", "IsInternallyDefined");
            DropColumn("dbo.TasksScheduler", "EntityId");
            DropColumn("dbo.TasksScheduler", "AverageRunTime");
            DropColumn("dbo.TaskSchedulerHistory", "LogDocumentId");
            DropColumn("dbo.RuleConditionFields", "ObjectFieldCode");
            DropColumn("dbo.RoleFeatures", "FeatureUniqeCode");
            DropColumn("dbo.Restrictions", "ObjectFieldCode");
            DropColumn("dbo.Reports", "AvailableForScheduling");
            DropColumn("dbo.Reports", "FeatureUniqeCode");
            DropColumn("dbo.ReportExecutionLogs", "ExecutedByServerName");
            DropColumn("dbo.ReportExecutionLogs", "StartDate");
            DropColumn("dbo.ReportExecutionLogs", "RetryNumber");
            DropColumn("dbo.ReconcileExternalPages", "EntityId");
            DropColumn("dbo.ReconcileExternalPages", "ObjectTableId");
            DropColumn("dbo.ReconcileExternalPageLines", "InReconcileProgress");
            DropColumn("dbo.ReconcileExternalPageLines", "InProgressExternalReconcile");
            DropColumn("dbo.QueryColumns", "ObjectFieldCode");
            DropColumn("dbo.QueryColumns", "QueryCode");
            DropColumn("dbo.PackageFeatures", "FeatureUniqeCode");
            DropColumn("dbo.ObjectTableTabs", "FeatureUniqeCode");
            DropColumn("dbo.ObjectTableTabs", "TabNameTextCodeCode");
            DropColumn("dbo.ObjectTableRules", "TriggerFieldCode");
            DropColumn("dbo.ObjectTableRuleFields", "ObjectFieldCode");
            DropColumn("dbo.ObjectTableHelperControls", "FeatureUniqeCode");
            DropColumn("dbo.ObjectFieldValidations", "ObjectFieldCode");
            DropColumn("dbo.ObjectFieldModifications", "ObjectFieldCode");
            DropColumn("dbo.MenusTables", "FeatureUniqeCode");
            DropColumn("dbo.MenuButtons", "LabelTextCodeCode");
            DropColumn("dbo.MenuButtons", "FeatureUniqeCode");
            DropColumn("dbo.LedgerTransactions", "InProgressExternalReconcile");
            DropColumn("dbo.JournalLines", "ActionId");
            DropColumn("dbo.FullAccountingSettings", "NumberOfAgingMonths");
            DropColumn("dbo.DocumentTypeTemplates", "BCC");
            DropColumn("dbo.CustomsInterfaceSettings", "AMCOceanStartDate");
            DropColumn("dbo.CustomsInterfaceSettings", "AMCAirStartDate");
            DropColumn("dbo.CustomerFieldsUpdateSettings", "ObjectFieldCode");
            DropColumn("dbo.CreditLimitSettings", "WarehousesInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "VendorsInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "TruckersInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "ShippingLinesInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "AirlinesInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "ShippingAgentsInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "CustomsAgentsInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "ShipperConsigneeInvoiceBlock");
            DropColumn("dbo.CreditLimitSettings", "AgentsInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "CustomersInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "WarehousesShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "VendorsShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "TruckersShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "ShippingLinesShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "AirlinesShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "ShippingAgentsShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "CustomsAgentsShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "ShipperConsigneeShipmentBlock");
            DropColumn("dbo.CreditLimitSettings", "AgentsShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "CustomersShipmentsBlock");
            DropColumn("dbo.BIReports", "LastRunByUserId");
            DropColumn("dbo.BIReports", "LastRunDate");
            DropColumn("dbo.GLAccounts", "Smallcashbook");
            DropColumn("dbo.GLAccounts", "NameForPrintingCheques");
            DropColumn("dbo.GLAccounts", "InterestCreditLimit");
            DropColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling");
            DropColumn("dbo.GLAccounts", "ActiveForInterestCreditInvoice");
            DropColumn("dbo.GLAccounts", "InterestCalculationStartDate");
            DropColumn("dbo.GLAccounts", "ActiveForInterest");
            DropColumn("dbo.GLAccounts", "AllowEditChequePayToName");
            DropColumn("dbo.BankAccounts", "PrintingAccountNumber");
            DropColumn("dbo.BankAccounts", "PrintingBranchNumber");
            DropColumn("dbo.ShipmentMasterDatas", "CutoffDate");
            DropColumn("dbo.ShipmentMasterDatas", "AutomaticLastUpdateDate");
            DropColumn("dbo.ShipmentComputedFields", "OperationallyClosedByUserName");
            DropColumn("dbo.ShipmentComputedFields", "PickupTo");
            DropColumn("dbo.ShipmentComputedFields", "PickupFrom");
            DropColumn("dbo.ShipmentComputedFields", "DeliveryTo");
            DropColumn("dbo.ShipmentComputedFields", "DeliveryFrom");
            DropColumn("dbo.ShipmentComputedFields", "DeliveryToPortId");
            DropColumn("dbo.ShipmentComputedFields", "LastPickupATD");
            DropColumn("dbo.ShipmentComputedFields", "LastPickupATA");
            DropColumn("dbo.ShipmentComputedFields", "LastPickupETD");
            DropColumn("dbo.ShipmentComputedFields", "LastPickupETA");
            DropColumn("dbo.ShipmentComputedFields", "NumberOfDeliveries");
            DropColumn("dbo.ShipmentComputedFields", "OperationallyClosedByUserId");
            DropColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumber");
            DropColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumberXMLData");
            DropColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumberUpdateDate");
            DropColumn("dbo.ShipmentAdditionalCloudDatas", "IsUserIDNumberRequired");
            DropColumn("dbo.EntityStatus", "AutomaticLastUpdateDate");
            DropColumn("dbo.Shipments", "WarehouseStorageFreeDays");
            DropColumn("dbo.Shipments", "FirstARInvoiceApprovalDate");
            DropColumn("dbo.Shipments", "CreatedByPartner");
            DropColumn("dbo.Shipments", "NotInvoicedReceivablesAmount");
            DropColumn("dbo.Shipments", "INTTRALastBookingResponse");
            DropColumn("dbo.Shipments", "INTTRABookingError");
            DropColumn("dbo.Shipments", "AutomaticLastUpdateDate");
            DropColumn("dbo.Shipments", "GrossWeightPerStorageDays");
            DropColumn("dbo.ARPayments", "Field10");
            DropColumn("dbo.ARPayments", "Field9");
            DropColumn("dbo.ARPayments", "Field8");
            DropColumn("dbo.ARPayments", "Field7");
            DropColumn("dbo.ARPayments", "Field6");
            DropColumn("dbo.ARPayments", "Field5");
            DropColumn("dbo.ARPayments", "Field4");
            DropColumn("dbo.ARPayments", "Field3");
            DropColumn("dbo.ARPayments", "Field2");
            DropColumn("dbo.ARPayments", "Field1");
            DropColumn("dbo.ARPayments", "IsPaymentNumberManuallySet");
            DropColumn("dbo.ARPayments", "CreatedByPartner");
            DropColumn("dbo.ARPayments", "OpenAmountInLocalCurrency");
            DropColumn("dbo.ARInvoices", "BillToGLAccountId");
            DropColumn("dbo.ARInvoices", "CreatedByPartner");
            DropColumn("dbo.ARInvoices", "DocumentFilingId");
            DropColumn("dbo.ARInvoices", "DateForInterest");
            DropColumn("dbo.AccountingPaymentMethods", "LocalName");
            DropColumn("dbo.APPayments", "Field10");
            DropColumn("dbo.APPayments", "Field9");
            DropColumn("dbo.APPayments", "Field8");
            DropColumn("dbo.APPayments", "Field7");
            DropColumn("dbo.APPayments", "Field6");
            DropColumn("dbo.APPayments", "Field5");
            DropColumn("dbo.APPayments", "Field4");
            DropColumn("dbo.APPayments", "Field3");
            DropColumn("dbo.APPayments", "Field2");
            DropColumn("dbo.APPayments", "Field1");
            DropColumn("dbo.APPayments", "CancelationNotes");
            DropColumn("dbo.APPayments", "DontIncludeInDeductionReport");
            DropColumn("dbo.APPayments", "AccountingCancelationDate");
            DropColumn("dbo.APPayments", "VendorIBANNumber");
            DropColumn("dbo.APPayments", "VendorSwift");
            DropColumn("dbo.APPayments", "VendorBankAccountNumber");
            DropColumn("dbo.APPayments", "VendorBankName");
            DropColumn("dbo.APPayments", "VendorBankAddress");
            DropColumn("dbo.ChargesGroups", "ViewOrder");
            DropColumn("dbo.ChargesTypes", "ApplyRegionalTax");
            DropColumn("dbo.APInvoiceLines", "Quantity");
            DropColumn("dbo.APInvoiceLines", "ContainerTypeId");
            DropColumn("dbo.APInvoices", "CreatedByPartner");
            DropColumn("dbo.AirlineMessagingRules", "RuleFieldCode");
            DropColumn("dbo.ShipmentLevels", "AutomaticLastUpdateDate");
            DropColumn("dbo.Features", "NameTextCodeCode");
            DropColumn("dbo.Features", "FeatureUniqeCode");
            DropColumn("dbo.Queries", "FeatureUniqeCode");
            DropColumn("dbo.Queries", "NameTextCodeCode");
            DropColumn("dbo.Queries", "OriginalQueryCode");
            DropColumn("dbo.Queries", "UniqueCode");
            DropColumn("dbo.ObjectFields", "RecordType");
            DropColumn("dbo.ObjectFields", "DisplayInAutomationAsEnitity");
            DropColumn("dbo.ObjectFields", "ShortNameTextCodeCode");
            DropColumn("dbo.ObjectFields", "ListTextCodeCode");
            DropColumn("dbo.ObjectFields", "HelpTextCodeCode");
            DropColumn("dbo.ObjectFields", "FullNameTextCodeCode");
            DropColumn("dbo.ObjectFields", "FieldCode");
            DropColumn("dbo.AdvancedQueryFilters", "ObjectFieldCode");
            DropColumn("dbo.AdvancedQueryFilters", "QueryCode");
            DropColumn("dbo.Tickets", "LastCorrespondence");
            DropColumn("dbo.Tickets", "SupportMailboxId");
            DropColumn("dbo.ShipmentTypes", "AutomaticLastUpdateDate");
            DropColumn("dbo.TransportModes", "AutomaticLastUpdateDate");
            DropColumn("dbo.Incoterms", "AutomaticLastUpdateDate");
            DropColumn("dbo.Ports", "AutomaticLastUpdateDate");
            DropColumn("dbo.Directions", "AutomaticLastUpdateDate");
            DropColumn("dbo.Quotes", "ProfitExchangeRate");
            DropColumn("dbo.Quotes", "ProfitCurrencyId");
            DropColumn("dbo.Quotes", "EstimatedProfitInProfit");
            DropColumn("dbo.Quotes", "EstimatedProfitInLocal");
            DropColumn("dbo.Quotes", "RequestDate");
            DropColumn("dbo.Quotes", "CountryForStatisticsId");
            DropColumn("dbo.Quotes", "Field20");
            DropColumn("dbo.Quotes", "Field19");
            DropColumn("dbo.Quotes", "Field18");
            DropColumn("dbo.Quotes", "Field17");
            DropColumn("dbo.Quotes", "Field16");
            DropColumn("dbo.Quotes", "Field15");
            DropColumn("dbo.Quotes", "Field14");
            DropColumn("dbo.Quotes", "Field13");
            DropColumn("dbo.Quotes", "Field12");
            DropColumn("dbo.Quotes", "Field11");
            DropColumn("dbo.Quotes", "QuoteHTMLDocumentId");
            DropColumn("dbo.Tips", "ShortTextCodeCode");
            DropColumn("dbo.ObjectTables", "NewButtonTextCodeCode");
            DropColumn("dbo.ObjectTables", "DescriptionTextCodeCode");
            DropColumn("dbo.ObjectTables", "HeaderScreenCode");
            DropColumn("dbo.States", "AutomaticLastUpdateDate");
            DropColumn("dbo.VatTypes", "IsRegionalTax");
            DropColumn("dbo.VatTypes", "RecognizedPercentage");
            DropColumn("dbo.VatTypes", "ReceivablesExternalId");
            DropColumn("dbo.VatTypes", "PayablesExternalId");
            DropColumn("dbo.ShippingLines", "INTTRAUpdatesShipment");
            DropColumn("dbo.PaymentTerms", "Code");
            DropColumn("dbo.PartnerTypes", "AutomaticLastUpdateDate");
            DropColumn("dbo.Countries", "AutomaticLastUpdateDate");
            DropColumn("dbo.Currencies", "AutomaticLastUpdateDate");
            DropColumn("dbo.Ranks", "AutomaticLastUpdateDate");
            DropColumn("dbo.Customers", "LastOpportunityStatus");
            DropColumn("dbo.Customers", "LastOpportunitySubject");
            DropColumn("dbo.Customers", "AutomaticLastUpdateDate");
            DropColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesContainers");
            DropColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesPackages");
            DropColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesPerContainers");
            DropColumn("dbo.QuoteTemplateSettings", "QuoteTemplatePDFMarginBottom");
            DropColumn("dbo.QuoteTemplateSettings", "QuoteTemplatePDFMarginTop");
            DropColumn("dbo.Departments", "AutomaticLastUpdateDate");
            DropColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_Via");
            DropColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_PartnerTypeId");
            DropColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_CardId");
            DropColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_ContactId");
            DropColumn("dbo.Contacts", "AutomaticLastUpdateDate");
            DropColumn("dbo.Branches", "AutomaticLastUpdateDate");
            DropColumn("dbo.Users", "AutomaticLastUpdateDate");
            DropColumn("dbo.Cards", "AutomaticLastUpdateDate");
            DropColumn("dbo.Cards", "StorageFreeDays");
            DropColumn("dbo.Cards", "CreatedByPartner");
            DropColumn("dbo.Addresses", "AutomaticLastUpdateDate");
            DropColumn("dbo.Tenants", "AutomaticLastUpdateDate");
            DropColumn("dbo.Tenants", "AllowCustomersInAgentsLOV");
            DropColumn("dbo.Tenants", "HideFCLAllIn");
            DropColumn("dbo.AccountingSettings", "AllowRegionalTaxManagement");
            DropColumn("dbo.AccountingSettings", "AllowManualARPaymentNumber");
            DropColumn("dbo.AccountingSettings", "QBOOAuth");
            DropColumn("dbo.AccountingSettings", "RefreshToken");
            DropTable("dbo.CustomsTransferLines");
            DropTable("dbo.CustomsTransferTypes");
            DropTable("dbo.CustomsTransferHeaders");
            DropTable("dbo.SupportMailboxes");
            DropTable("dbo.LogBoxTenantSettings");
            DropTable("dbo.SharedLogisticsContactLastLogins");
            DropTable("dbo.AccountingPartners");
            AddPrimaryKey("dbo.CustomerCompetitorProducts", "ProductTypeCode");
            CreateIndex("dbo.SchedulerLogs", "HistoryId");
            CreateIndex("dbo.ReconcileExternalPages", "BankAccountId");
            CreateIndex("dbo.JournalLines", "ActionCode");
            CreateIndex("dbo.APPayments", "ApprovedByUserId");
            CreateIndex("dbo.APPayments", "AccountingPaymentMethodId");
            CreateIndex("dbo.APInvoiceLines", "ForiegnCurrencyId");
            CreateIndex("dbo.Tenants", "LogBoxAdminUserId");
            CreateIndex("dbo.Tenants", "PasswordPolicyCode");
            AddForeignKey("dbo.JournalLines", "ActionCode", "dbo.JournalActionTypes", "Id");
            AddForeignKey("dbo.SchedulerLogs", "HistoryId", "dbo.TaskSchedulerHistory", "Id");
            AddForeignKey("dbo.ReconcileExternalPages", "BankAccountId", "dbo.BankAccounts", "Id");
            AddForeignKey("dbo.Tenants", "LogBoxAdminUserId", "dbo.Contacts", "Id");
        }
    }
}
