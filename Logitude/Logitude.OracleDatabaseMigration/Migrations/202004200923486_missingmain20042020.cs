namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingmain20042020 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.APPayments", "PaymentMethodId", "dbo.APPaymentMethods");
            DropForeignKey("dbo.BIReports", "LastRunByUserId", "dbo.Users");
            DropIndex("dbo.APPayments", new[] { "PaymentMethodId" });
            DropIndex("dbo.BIReports", new[] { "LastRunByUserId" });
            DropIndex("dbo.Reports", new[] { "ReportGroupId" });
            DropPrimaryKey("dbo.DWObjectFieldCategories");
            CreateTable(
                "dbo.LastRunDetails",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Tenant = c.Int(nullable: false),
                        LastRunDate = c.DateTime(nullable: false, precision: 7),
                        LastRunByUserId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.LastRunByUserId)
                .Index(t => t.LastRunByUserId);
            
            AddColumn("dbo.Users", "LayoutDirection", c => c.String(maxLength: 3, unicode: false));
            AddColumn("dbo.Documents", "MarkForDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowVATTypePackages", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowVATTypeContainers", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowVATPercentagePackages", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowVATPercentageContainers", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "DisplayDocumentsAndEvents", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccountingSettings", "EnableAPPaymentExternalPayment", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccountingSettings", "TransferToFTPActivated", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccountingSettings", "TransferFTPDetailId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.AccountingSystems", "CanTransferToFTP", c => c.Boolean(nullable: false));
            AddColumn("dbo.ObjectTables", "HashString", c => c.String());
            AddColumn("dbo.Ports", "CountryCode", c => c.String(maxLength: 2, unicode: false));
            AddColumn("dbo.Ports", "CountryName", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.Ports", "StateCode", c => c.String(maxLength: 10, unicode: false));
            AddColumn("dbo.APInvoices", "Field1", c => c.String(maxLength: 250));
            AddColumn("dbo.APInvoices", "Field2", c => c.String(maxLength: 250));
            AddColumn("dbo.APInvoices", "Field3", c => c.String(maxLength: 250));
            AddColumn("dbo.APInvoices", "Field4", c => c.String(maxLength: 250));
            AddColumn("dbo.APInvoices", "Field5", c => c.String(maxLength: 250));
            AddColumn("dbo.APInvoices", "Field6", c => c.String(maxLength: 250));
            AddColumn("dbo.APInvoices", "Field7", c => c.String(maxLength: 250));
            AddColumn("dbo.APInvoices", "Field8", c => c.String(maxLength: 250));
            AddColumn("dbo.APInvoices", "Field9", c => c.String(maxLength: 250));
            AddColumn("dbo.APInvoices", "Field10", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "ExternalPaymentAmount", c => c.Double());
            AddColumn("dbo.APPayments", "ExternalPaymentDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.APPayments", "ExternalPaymentNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.ARInvoices", "RegionalTaxId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ARInvoices", "RegionalTaxPercentage", c => c.Double());
            AddColumn("dbo.ARInvoiceLines", "IsRegionalTax", c => c.Boolean(nullable: false));
            AddColumn("dbo.ARPayments", "AccountingCancelationDate", c => c.DateTime(precision: 7));
            AddColumn("dbo.ARPayments", "CancelationNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.ARInvoiceTotalVATs", "IsRegionalTax", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shipments", "SLAC", c => c.String(maxLength: 5, unicode: false));
            AddColumn("dbo.BIReports", "FactTableName", c => c.String(nullable: false, maxLength: 30, unicode: false));
            AddColumn("dbo.BIReports", "LastRunId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.CustomerFieldsUpdateSettings", "SearchFields", c => c.String(maxLength: 1000));
            AddColumn("dbo.DWObjectFieldCategories", "DWObjectTableCode", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AddColumn("dbo.ShipmentPackages", "WarehouseReleaseNumber", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.QuoteCharges", "IsCostAllIn", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteCharges", "TariffId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.QuoteCharges", "TariffNumber", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.QuoteCharges", "TariffVersion", c => c.Int(nullable: false));
            AddColumn("dbo.ReportExecutionLogs", "ExcelOnly", c => c.Boolean(nullable: false));
            AddColumn("dbo.Reports", "ExcelOnly", c => c.Boolean(nullable: false));
            AddColumn("dbo.TariffSettings", "ContainerDefaults", c => c.String(maxLength: 50, unicode: false));
            AddColumn("dbo.WarehouseReleasePackages", "IsCanceled", c => c.Boolean(nullable: false));
            AddColumn("dbo.WarehouseReleases", "TruckerId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "TruckerReference", c => c.String(maxLength: 15, unicode: false));
            AddPrimaryKey("dbo.DWObjectFieldCategories", "Id");
            CreateIndex("dbo.AccountingSettings", "TransferFTPDetailId");
            CreateIndex("dbo.Tickets", "QuoteId");
            CreateIndex("dbo.ARInvoices", "RegionalTaxId");
            CreateIndex("dbo.BIReports", "LastRunId");
            CreateIndex("dbo.Reports", "ReportGroupId");
            CreateIndex("dbo.WarehouseReleases", "TruckerId");
            AddForeignKey("dbo.AccountingSettings", "TransferFTPDetailId", "dbo.FTPDetails", "Id");
            AddForeignKey("dbo.Tickets", "QuoteId", "dbo.Quotes", "Id");
            AddForeignKey("dbo.ARInvoices", "RegionalTaxId", "dbo.VatTypes", "Id");
            AddForeignKey("dbo.BIReports", "LastRunId", "dbo.LastRunDetails", "Id");
            AddForeignKey("dbo.WarehouseReleases", "TruckerId", "dbo.Cards", "Id");
            DropColumn("dbo.APPayments", "PaymentMethodId");
            DropColumn("dbo.BIReports", "LastRunDate");
            DropColumn("dbo.BIReports", "LastRunByUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BIReports", "LastRunByUserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AddColumn("dbo.BIReports", "LastRunDate", c => c.DateTime(nullable: false, precision: 7));
            AddColumn("dbo.APPayments", "PaymentMethodId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            DropForeignKey("dbo.WarehouseReleases", "TruckerId", "dbo.Cards");
            DropForeignKey("dbo.BIReports", "LastRunId", "dbo.LastRunDetails");
            DropForeignKey("dbo.LastRunDetails", "LastRunByUserId", "dbo.Users");
            DropForeignKey("dbo.ARInvoices", "RegionalTaxId", "dbo.VatTypes");
            DropForeignKey("dbo.Tickets", "QuoteId", "dbo.Quotes");
            DropForeignKey("dbo.AccountingSettings", "TransferFTPDetailId", "dbo.FTPDetails");
            DropIndex("dbo.WarehouseReleases", new[] { "TruckerId" });
            DropIndex("dbo.Reports", new[] { "ReportGroupId" });
            DropIndex("dbo.LastRunDetails", new[] { "LastRunByUserId" });
            DropIndex("dbo.BIReports", new[] { "LastRunId" });
            DropIndex("dbo.ARInvoices", new[] { "RegionalTaxId" });
            DropIndex("dbo.Tickets", new[] { "QuoteId" });
            DropIndex("dbo.AccountingSettings", new[] { "TransferFTPDetailId" });
            DropPrimaryKey("dbo.DWObjectFieldCategories");
            AlterColumn("dbo.Reports", "ReportGroupId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.DWObjectFieldCategories", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Quotes", "CustomerName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.AccountingSettings", "QBOAccessTokenSecret", c => c.String(maxLength: 200));
            AlterColumn("dbo.AccountingSettings", "QBOAccessToken", c => c.String(maxLength: 200));
            DropColumn("dbo.WarehouseReleases", "TruckerReference");
            DropColumn("dbo.WarehouseReleases", "TruckerId");
            DropColumn("dbo.WarehouseReleasePackages", "IsCanceled");
            DropColumn("dbo.TariffSettings", "ContainerDefaults");
            DropColumn("dbo.Reports", "ExcelOnly");
            DropColumn("dbo.ReportExecutionLogs", "ExcelOnly");
            DropColumn("dbo.QuoteCharges", "TariffVersion");
            DropColumn("dbo.QuoteCharges", "TariffNumber");
            DropColumn("dbo.QuoteCharges", "TariffId");
            DropColumn("dbo.QuoteCharges", "IsCostAllIn");
            DropColumn("dbo.ShipmentPackages", "WarehouseReleaseNumber");
            DropColumn("dbo.DWObjectFieldCategories", "DWObjectTableCode");
            DropColumn("dbo.CustomerFieldsUpdateSettings", "SearchFields");
            DropColumn("dbo.BIReports", "LastRunId");
            DropColumn("dbo.BIReports", "FactTableName");
            DropColumn("dbo.Shipments", "SLAC");
            DropColumn("dbo.ARInvoiceTotalVATs", "IsRegionalTax");
            DropColumn("dbo.ARPayments", "CancelationNotes");
            DropColumn("dbo.ARPayments", "AccountingCancelationDate");
            DropColumn("dbo.ARInvoiceLines", "IsRegionalTax");
            DropColumn("dbo.ARInvoices", "RegionalTaxPercentage");
            DropColumn("dbo.ARInvoices", "RegionalTaxId");
            DropColumn("dbo.APPayments", "ExternalPaymentNotes");
            DropColumn("dbo.APPayments", "ExternalPaymentDate");
            DropColumn("dbo.APPayments", "ExternalPaymentAmount");
            DropColumn("dbo.APInvoices", "Field10");
            DropColumn("dbo.APInvoices", "Field9");
            DropColumn("dbo.APInvoices", "Field8");
            DropColumn("dbo.APInvoices", "Field7");
            DropColumn("dbo.APInvoices", "Field6");
            DropColumn("dbo.APInvoices", "Field5");
            DropColumn("dbo.APInvoices", "Field4");
            DropColumn("dbo.APInvoices", "Field3");
            DropColumn("dbo.APInvoices", "Field2");
            DropColumn("dbo.APInvoices", "Field1");
            DropColumn("dbo.Ports", "StateCode");
            DropColumn("dbo.Ports", "CountryName");
            DropColumn("dbo.Ports", "CountryCode");
            DropColumn("dbo.ObjectTables", "HashString");
            DropColumn("dbo.AccountingSystems", "CanTransferToFTP");
            DropColumn("dbo.AccountingSettings", "TransferFTPDetailId");
            DropColumn("dbo.AccountingSettings", "TransferToFTPActivated");
            DropColumn("dbo.AccountingSettings", "EnableAPPaymentExternalPayment");
            DropColumn("dbo.Tenants", "DisplayDocumentsAndEvents");
            DropColumn("dbo.QuoteTemplateSettings", "ShowVATPercentageContainers");
            DropColumn("dbo.QuoteTemplateSettings", "ShowVATPercentagePackages");
            DropColumn("dbo.QuoteTemplateSettings", "ShowVATTypeContainers");
            DropColumn("dbo.QuoteTemplateSettings", "ShowVATTypePackages");
            DropColumn("dbo.Documents", "MarkForDelete");
            DropColumn("dbo.Users", "LayoutDirection");
            DropTable("dbo.LastRunDetails");
            AddPrimaryKey("dbo.DWObjectFieldCategories", "Id");
            CreateIndex("dbo.Reports", "ReportGroupId");
            CreateIndex("dbo.BIReports", "LastRunByUserId");
            CreateIndex("dbo.APPayments", "PaymentMethodId");
            AddForeignKey("dbo.BIReports", "LastRunByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.APPayments", "PaymentMethodId", "dbo.APPaymentMethods", "Id");
        }
    }
}
