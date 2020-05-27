namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GLAccountNameForPrintingChequesMigration : DbMigration
    {
        public override void Up()
        {
           // AddColumn("dbo.GLAccounts", "InterestCreditLimit", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.GLAccounts", "NameForPrintingCheques", c => c.String(maxLength: 1000));
           
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AirlineAreasPorts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AirlineAreaId = c.String(nullable: false, maxLength: 128),
                        PortId = c.String(maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        AddedDate = c.DateTime(),
                        AddedByUserId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AirlineAreas",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AirlineId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Description = c.String(),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        CreateDate = c.DateTime(),
                        UpdateDate = c.DateTime(),
                        CreatedByUserId = c.String(maxLength: 15, unicode: false),
                        UpdatedByUserId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ARInvoices", "BillToGLAccountId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ARInvoices", "DateForVATInterest", c => c.DateTime());
            DropForeignKey("dbo.TariffLinesContainersPrices", "TariffLineId", "dbo.TariffLines");
            DropForeignKey("dbo.TariffLinesContainersPrices", "TariffId", "dbo.Tariffs");
            DropForeignKey("dbo.TariffLinesContainersPrices", "SurchargeId", "dbo.ChargesTypes");
            DropForeignKey("dbo.InterestReportLinesByDates", "InterestReportId", "dbo.InterestReports");
            DropForeignKey("dbo.InterestReportLines", "InterestTransactionId", "dbo.InterestTransactions");
            DropForeignKey("dbo.InterestTransactions", "InterestEntityTypeCode", "dbo.InterestEntityTypes");
            DropForeignKey("dbo.InterestTransactions", "GLAccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.InterestTransactions", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.InterestReportLines", "InterestReportId", "dbo.InterestReports");
            DropForeignKey("dbo.InterestReports", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.InterestReports", "InterestReportStatusCode", "dbo.InterestReportStatuses");
            DropForeignKey("dbo.InterestReports", "GLAccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.InterestReports", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.InterestReports", "ARinvoiceId", "dbo.ARInvoices");
            DropForeignKey("dbo.InterestBasesPeriods", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.InterestBasesPeriods", "InterestBaseTypeId", "dbo.InterestBasesTypes");
            DropForeignKey("dbo.InterestBasesPeriods", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.GLAccountInterestPeriods", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.GLAccountInterestPeriods", "StandardInterestRateBaseId", "dbo.InterestBasesTypes");
            DropForeignKey("dbo.GLAccountInterestPeriods", "GLAccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.GLAccountInterestPeriods", "ExceptionalInterestRateBaseId", "dbo.InterestBasesTypes");
            DropForeignKey("dbo.GLAccountInterestPeriods", "CreditInterestRateBaseId", "dbo.InterestBasesTypes");
            DropForeignKey("dbo.InterestBasesTypes", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.InterestBasesTypes", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.GLAccountInterestPeriods", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.CustomerOpenFilesAmounts", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CarrierAreasPorts", "PortId", "dbo.Ports");
            DropForeignKey("dbo.CarrierAreasPorts", "CarrierAreaId", "dbo.CarrierAreas");
            DropForeignKey("dbo.CarrierAreasPorts", "AddedByUserId", "dbo.Users");
            DropForeignKey("dbo.CarrierAreas", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.CarrierAreas", "TransportModeCode", "dbo.TransportModes");
            DropForeignKey("dbo.CarrierAreas", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.CarrierAreas", "CarrierId", "dbo.Cards");
            DropForeignKey("dbo.BIReports", "LastRunByUserId", "dbo.Users");
            DropForeignKey("dbo.Tickets", "SupportMailboxId", "dbo.SupportMailboxes");
            DropForeignKey("dbo.SupportMailboxes", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.SupportMailboxes", "CreatedByUserId", "dbo.Users");
            DropIndex("dbo.TariffLinesContainersPrices", new[] { "SurchargeId" });
            DropIndex("dbo.TariffLinesContainersPrices", new[] { "TariffLineId" });
            DropIndex("dbo.TariffLinesContainersPrices", new[] { "TariffId" });
            DropIndex("dbo.InterestReportLinesByDates", new[] { "InterestReportId" });
            DropIndex("dbo.InterestTransactions", new[] { "CurrencyId" });
            DropIndex("dbo.InterestTransactions", new[] { "InterestEntityTypeCode" });
            DropIndex("dbo.InterestTransactions", new[] { "GLAccountId" });
            DropIndex("dbo.InterestReports", new[] { "InterestReportStatusCode" });
            DropIndex("dbo.InterestReports", new[] { "ARinvoiceId" });
            DropIndex("dbo.InterestReports", new[] { "GLAccountId" });
            DropIndex("dbo.InterestReports", new[] { "UpdatedByUserId" });
            DropIndex("dbo.InterestReports", new[] { "CreatedByUserId" });
            DropIndex("dbo.InterestReportLines", new[] { "InterestTransactionId" });
            DropIndex("dbo.InterestReportLines", new[] { "InterestReportId" });
            DropIndex("dbo.InterestBasesPeriods", new[] { "UpdatedByUserId" });
            DropIndex("dbo.InterestBasesPeriods", new[] { "CreatedByUserId" });
            DropIndex("dbo.InterestBasesPeriods", new[] { "InterestBaseTypeId" });
            DropIndex("dbo.InterestBasesTypes", new[] { "UpdatedByUserId" });
            DropIndex("dbo.InterestBasesTypes", new[] { "CreatedByUserId" });
            DropIndex("dbo.GLAccountInterestPeriods", new[] { "CreatedByUserId" });
            DropIndex("dbo.GLAccountInterestPeriods", new[] { "UpdatedByUserId" });
            DropIndex("dbo.GLAccountInterestPeriods", new[] { "CreditInterestRateBaseId" });
            DropIndex("dbo.GLAccountInterestPeriods", new[] { "ExceptionalInterestRateBaseId" });
            DropIndex("dbo.GLAccountInterestPeriods", new[] { "StandardInterestRateBaseId" });
            DropIndex("dbo.GLAccountInterestPeriods", new[] { "GLAccountId" });
            DropIndex("dbo.CustomerOpenFilesAmounts", new[] { "CustomerId" });
            DropIndex("dbo.CarrierAreasPorts", new[] { "AddedByUserId" });
            DropIndex("dbo.CarrierAreasPorts", new[] { "PortId" });
            DropIndex("dbo.CarrierAreasPorts", new[] { "CarrierAreaId" });
            DropIndex("dbo.CarrierAreas", new[] { "TransportModeCode" });
            DropIndex("dbo.CarrierAreas", new[] { "UpdatedByUserId" });
            DropIndex("dbo.CarrierAreas", new[] { "CreatedByUserId" });
            DropIndex("dbo.CarrierAreas", new[] { "CarrierId" });
            DropIndex("dbo.BIReports", new[] { "LastRunByUserId" });
            DropIndex("dbo.Features", new[] { "FeatureUniqeCode" });
            DropIndex("dbo.ObjectFields", new[] { "FieldCode" });
            DropIndex("dbo.SupportMailboxes", new[] { "UpdatedByUserId" });
            DropIndex("dbo.SupportMailboxes", new[] { "CreatedByUserId" });
            DropIndex("dbo.Tickets", new[] { "SupportMailboxId" });
            DropColumn("dbo.Translations", "TextCodeCode");
            DropColumn("dbo.SLAHeaders", "SearchFields");
            DropColumn("dbo.ScreenModifications", "ScreenCode");
            DropColumn("dbo.ScreenFields", "ObjectFieldCode");
            DropColumn("dbo.ScreenFields", "ScreenCode");
            DropColumn("dbo.RuleConditionFields", "ObjectFieldCode");
            DropColumn("dbo.RoleFeatures", "FeatureUniqeCode");
            DropColumn("dbo.Restrictions", "ObjectFieldCode");
            DropColumn("dbo.Reports", "FeatureUniqeCode");
            DropColumn("dbo.QueryColumns", "ObjectFieldCode");
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
            DropColumn("dbo.CustomerFieldsUpdateSettings", "ObjectFieldCode");
            DropColumn("dbo.BIReports", "LastRunByUserId");
            DropColumn("dbo.BIReports", "LastRunDate");
            DropColumn("dbo.GLAccounts", "NameForPrintingCheques");
            DropColumn("dbo.GLAccounts", "InterestCreditLimit");
            DropColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling");
            DropColumn("dbo.GLAccounts", "ActiveForInterestCreditInvoice");
            DropColumn("dbo.GLAccounts", "InterestCalculationStartDate");
            DropColumn("dbo.GLAccounts", "ActiveForInterest");
            DropColumn("dbo.ARInvoices", "DateForInterest");
            DropColumn("dbo.APPayments", "CancelationNotes");
            DropColumn("dbo.APPayments", "DontIncludeInDeductionReport");
            DropColumn("dbo.APPayments", "AccountingCancelationDate");
            DropColumn("dbo.AirlineMessagingRules", "RuleFieldCode");
            DropColumn("dbo.Features", "NameTextCodeCode");
            DropColumn("dbo.Features", "FeatureUniqeCode");
            DropColumn("dbo.Queries", "FeatureUniqeCode");
            DropColumn("dbo.Queries", "NameTextCodeCode");
            DropColumn("dbo.ObjectFields", "ShortNameTextCodeCode");
            DropColumn("dbo.ObjectFields", "ListTextCodeCode");
            DropColumn("dbo.ObjectFields", "HelpTextCodeCode");
            DropColumn("dbo.ObjectFields", "FullNameTextCodeCode");
            DropColumn("dbo.ObjectFields", "FieldCode");
            DropColumn("dbo.AdvancedQueryFilters", "ObjectFieldCode");
            DropColumn("dbo.Tickets", "SupportMailboxId");
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
            DropColumn("dbo.Tips", "ShortTextCodeCode");
            DropColumn("dbo.ObjectTables", "NewButtonTextCodeCode");
            DropColumn("dbo.ObjectTables", "DescriptionTextCodeCode");
            DropColumn("dbo.ObjectTables", "HeaderScreenCode");
            DropTable("dbo.TariffLinesContainersPrices");
            DropTable("dbo.InterestReportLinesByDates");
            DropTable("dbo.InterestTransactions");
            DropTable("dbo.InterestReportStatuses");
            DropTable("dbo.InterestReports");
            DropTable("dbo.InterestReportLines");
            DropTable("dbo.InterestEntityTypes");
            DropTable("dbo.InterestBasesPeriods");
            DropTable("dbo.InterestBasesTypes");
            DropTable("dbo.GLAccountInterestPeriods");
            DropTable("dbo.CustomerOpenFilesAmounts");
            DropTable("dbo.CarrierAreasPorts");
            DropTable("dbo.CarrierAreas");
            DropTable("dbo.SupportMailboxes");
            CreateIndex("dbo.AirlineAreasPorts", "AddedByUserId");
            CreateIndex("dbo.AirlineAreasPorts", "PortId");
            CreateIndex("dbo.AirlineAreasPorts", "AirlineAreaId");
            CreateIndex("dbo.AirlineAreas", "UpdatedByUserId");
            CreateIndex("dbo.AirlineAreas", "CreatedByUserId");
            CreateIndex("dbo.AirlineAreas", "AirlineId");
            AddForeignKey("dbo.AirlineAreasPorts", "PortId", "dbo.Ports", "Id");
            AddForeignKey("dbo.AirlineAreasPorts", "AirlineAreaId", "dbo.AirlineAreas", "Id");
            AddForeignKey("dbo.AirlineAreasPorts", "AddedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.AirlineAreas", "UpdatedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.AirlineAreas", "CreatedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.AirlineAreas", "AirlineId", "dbo.Airlines", "Id");
        }
    }
}
