namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GLAccountNameForPrintingchequesMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AirlineAreas", "AirlineId", "dbo.Airlines");
            DropForeignKey("dbo.AirlineAreas", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.AirlineAreas", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.AirlineAreasPorts", "AddedByUserId", "dbo.Users");
            DropForeignKey("dbo.AirlineAreasPorts", "AirlineAreaId", "dbo.AirlineAreas");
            DropForeignKey("dbo.AirlineAreasPorts", "PortId", "dbo.Ports");
            DropIndex("dbo.AirlineAreas", new[] { "AirlineId" });
            DropIndex("dbo.AirlineAreas", new[] { "CreatedByUserId" });
            DropIndex("dbo.AirlineAreas", new[] { "UpdatedByUserId" });
            DropIndex("dbo.AirlineAreasPorts", new[] { "AirlineAreaId" });
            DropIndex("dbo.AirlineAreasPorts", new[] { "PortId" });
            DropIndex("dbo.AirlineAreasPorts", new[] { "AddedByUserId" });
            CreateTable(
                "dbo.SupportMailboxes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Mailbox = c.String(nullable: false, maxLength: 100, unicode: false),
                        Inactive = c.Boolean(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                        SearchFields = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
            CreateTable(
                "dbo.CarrierAreas",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        CarrierId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Description = c.String(),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        CreateDate = c.DateTime(),
                        UpdateDate = c.DateTime(),
                        CreatedByUserId = c.String(maxLength: 15, unicode: false),
                        UpdatedByUserId = c.String(maxLength: 15, unicode: false),
                        TransportModeCode = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cards", t => t.CarrierId)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.TransportModes", t => t.TransportModeCode)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CarrierId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.TransportModeCode);
            
            CreateTable(
                "dbo.CarrierAreasPorts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CarrierAreaId = c.String(nullable: false, maxLength: 15, unicode: false),
                        PortId = c.String(maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        AddedDate = c.DateTime(),
                        AddedByUserId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AddedByUserId)
                .ForeignKey("dbo.CarrierAreas", t => t.CarrierAreaId)
                .ForeignKey("dbo.Ports", t => t.PortId)
                .Index(t => t.CarrierAreaId)
                .Index(t => t.PortId)
                .Index(t => t.AddedByUserId);
            
            CreateTable(
                "dbo.CustomerOpenFilesAmounts",
                c => new
                    {
                        CustomerId = c.String(nullable: false, maxLength: 15, unicode: false),
                        TotalOpenFilesAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tenant = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerId)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.GLAccountInterestPeriods",
                c => new
                    {
                        LineNumber = c.Int(nullable: false),
                        GLAccountId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        PeriodStartDate = c.DateTime(nullable: false),
                        StandardInterestRateBaseId = c.String(nullable: false, maxLength: 15, unicode: false),
                        StandardAddInterestPercent = c.Decimal(nullable: false, precision: 4, scale: 2),
                        ExceptionalInterestRateBaseId = c.String(nullable: false, maxLength: 15, unicode: false),
                        ExceptionalAddInterestPercent = c.Decimal(nullable: false, precision: 4, scale: 2),
                        CreditInterestRateBaseId = c.String(nullable: false, maxLength: 15, unicode: false),
                        CreditAddInterestPercent = c.Decimal(nullable: false, precision: 4, scale: 2),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDateTime = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.LineNumber, t.GLAccountId })
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.InterestBasesTypes", t => t.CreditInterestRateBaseId)
                .ForeignKey("dbo.InterestBasesTypes", t => t.ExceptionalInterestRateBaseId)
                .ForeignKey("dbo.GLAccounts", t => t.GLAccountId)
                .ForeignKey("dbo.InterestBasesTypes", t => t.StandardInterestRateBaseId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.GLAccountId)
                .Index(t => t.StandardInterestRateBaseId)
                .Index(t => t.ExceptionalInterestRateBaseId)
                .Index(t => t.CreditInterestRateBaseId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId);
            
            CreateTable(
                "dbo.InterestBasesTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(maxLength: 15, unicode: false),
                        SearchFields = c.String(),
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        LocalName = c.String(maxLength: 256),
                        EnglishName = c.String(maxLength: 256, unicode: false),
                        Description = c.String(maxLength: 1024),
                        InActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
            CreateTable(
                "dbo.InterestBasesPeriods",
                c => new
                    {
                        InterestBaseTypeId = c.String(nullable: false, maxLength: 15, unicode: false),
                        LineNumber = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(maxLength: 15, unicode: false),
                        InterestBaseStartDate = c.DateTime(nullable: false),
                        InterestRate = c.Decimal(nullable: false, precision: 4, scale: 2),
                    })
                .PrimaryKey(t => new { t.InterestBaseTypeId, t.LineNumber })
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.InterestBasesTypes", t => t.InterestBaseTypeId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.InterestBaseTypeId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
            CreateTable(
                "dbo.InterestEntityTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 1, unicode: false),
                        EnglishName = c.String(maxLength: 100, unicode: false),
                        SearchFields = c.String(),
                        LocalName = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.InterestReportLines",
                c => new
                    {
                        InterestReportId = c.String(nullable: false, maxLength: 15, unicode: false),
                        InterestTransactionId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.InterestReportId, t.InterestTransactionId })
                .ForeignKey("dbo.InterestReports", t => t.InterestReportId)
                .ForeignKey("dbo.InterestTransactions", t => t.InterestTransactionId)
                .Index(t => t.InterestReportId)
                .Index(t => t.InterestTransactionId);
            
            CreateTable(
                "dbo.InterestReports",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(),
                        CreatedByUserId = c.String(maxLength: 15, unicode: false),
                        UpdateDateTime = c.DateTime(),
                        UpdatedByUserId = c.String(maxLength: 15, unicode: false),
                        GLAccountId = c.String(nullable: false, maxLength: 15, unicode: false),
                        ReportNumber = c.String(maxLength: 15, unicode: false),
                        InterestCalculationDate = c.DateTime(nullable: false),
                        TotalAmount = c.Decimal(precision: 18, scale: 2),
                        OpenBalance = c.Decimal(precision: 18, scale: 2),
                        CloseBalance = c.Decimal(precision: 18, scale: 2),
                        ARinvoiceId = c.String(maxLength: 15, unicode: false),
                        InvoiceAmount = c.Decimal(precision: 18, scale: 2),
                        GLAccountInterestCreditLimit = c.Decimal(precision: 18, scale: 2),
                        InterestReportStatusCode = c.String(maxLength: 4, unicode: false),
                        SearchFields = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ARInvoices", t => t.ARinvoiceId)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.GLAccounts", t => t.GLAccountId)
                .ForeignKey("dbo.InterestReportStatuses", t => t.InterestReportStatusCode)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.GLAccountId)
                .Index(t => t.ARinvoiceId)
                .Index(t => t.InterestReportStatusCode);
            
            CreateTable(
                "dbo.InterestReportStatuses",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        LocalName = c.String(maxLength: 30),
                        SearchFields = c.String(),
                        EnglishName = c.String(nullable: false, maxLength: 60, unicode: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.InterestTransactions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        UpdateDateTime = c.DateTime(nullable: false),
                        SearchFields = c.String(),
                        GLAccountId = c.String(maxLength: 15, unicode: false),
                        InterestEntityTypeCode = c.String(nullable: false, maxLength: 1, unicode: false),
                        EntityId = c.String(nullable: false, maxLength: 15, unicode: false),
                        OriginalEntityLineNumber = c.Int(nullable: false),
                        LocalAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                        ForeignAmount = c.Decimal(precision: 16, scale: 2),
                        CurrencyId = c.String(maxLength: 15, unicode: false),
                        InterestValueDate = c.DateTime(nullable: false),
                        InterestReportId = c.String(maxLength: 15, unicode: false),
                        IsClosed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .ForeignKey("dbo.GLAccounts", t => t.GLAccountId)
                .ForeignKey("dbo.InterestEntityTypes", t => t.InterestEntityTypeCode)
                .Index(t => t.GLAccountId)
                .Index(t => t.InterestEntityTypeCode)
                .Index(t => t.CurrencyId);
            
            CreateTable(
                "dbo.InterestReportLinesByDates",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        InterestReportId = c.String(nullable: false, maxLength: 15, unicode: false),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        TotalInterestDays = c.Int(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AccumulatedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StandardInterestPercentage = c.Decimal(nullable: false, precision: 4, scale: 2),
                        ExceptionalInterestPercentage = c.Decimal(nullable: false, precision: 4, scale: 2),
                        CreditInterestPercentage = c.Decimal(nullable: false, precision: 4, scale: 2),
                        StandardInterestAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExceptionalInterestAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditInterestAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CalculatedStandInterestAmount = c.Decimal(nullable: false, precision: 20, scale: 4),
                        CalculatedExcepInterestAmount = c.Decimal(nullable: false, precision: 20, scale: 4),
                        CalculatedCreditInterestAmount = c.Decimal(nullable: false, precision: 20, scale: 4),
                        CalculationDetails = c.String(maxLength: 256),
                        LineNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InterestReports", t => t.InterestReportId)
                .Index(t => t.InterestReportId);
            
            CreateTable(
                "dbo.TariffLinesContainersPrices",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        TariffId = c.String(nullable: false, maxLength: 15, unicode: false),
                        TariffLineId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SurchargeId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Price1 = c.Decimal(precision: 18, scale: 3),
                        Price2 = c.Decimal(precision: 18, scale: 3),
                        Price3 = c.Decimal(precision: 18, scale: 3),
                        Price4 = c.Decimal(precision: 18, scale: 3),
                        Price5 = c.Decimal(precision: 18, scale: 3),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChargesTypes", t => t.SurchargeId)
                .ForeignKey("dbo.Tariffs", t => t.TariffId)
                .ForeignKey("dbo.TariffLines", t => t.TariffLineId)
                .Index(t => t.TariffId)
                .Index(t => t.TariffLineId)
                .Index(t => t.SurchargeId);
            
            AddColumn("dbo.ObjectTables", "HeaderScreenCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectTables", "DescriptionTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectTables", "NewButtonTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Tips", "ShortTextCodeCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
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
            AddColumn("dbo.Tickets", "SupportMailboxId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.AdvancedQueryFilters", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AddColumn("dbo.ObjectFields", "FieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AddColumn("dbo.ObjectFields", "FullNameTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectFields", "HelpTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectFields", "ListTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectFields", "ShortNameTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Queries", "NameTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Queries", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.Features", "FeatureUniqeCode", c => c.String(nullable: false, maxLength: 120, unicode: false));
            AddColumn("dbo.Features", "NameTextCodeCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AddColumn("dbo.AirlineMessagingRules", "RuleFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AddColumn("dbo.APPayments", "AccountingCancelationDate", c => c.DateTime());
            AddColumn("dbo.APPayments", "DontIncludeInDeductionReport", c => c.Boolean(nullable: false));
            AddColumn("dbo.APPayments", "CancelationNotes", c => c.String());
            AddColumn("dbo.ARInvoices", "DateForInterest", c => c.DateTime());
            AddColumn("dbo.GLAccounts", "ActiveForInterest", c => c.Boolean());
            AddColumn("dbo.GLAccounts", "InterestCalculationStartDate", c => c.DateTime());
            AddColumn("dbo.GLAccounts", "ActiveForInterestCreditInvoice", c => c.Boolean());
            AddColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling", c => c.Int());
            AddColumn("dbo.GLAccounts", "InterestCreditLimit", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.BIReports", "LastRunDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.BIReports", "LastRunByUserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AddColumn("dbo.CustomerFieldsUpdateSettings", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.MenuButtons", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.MenuButtons", "LabelTextCodeCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AddColumn("dbo.MenusTables", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.ObjectFieldModifications", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AddColumn("dbo.ObjectFieldValidations", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AddColumn("dbo.ObjectTableHelperControls", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.ObjectTableRuleFields", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AddColumn("dbo.ObjectTableRules", "TriggerFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ObjectTableTabs", "TabNameTextCodeCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectTableTabs", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.PackageFeatures", "FeatureUniqeCode", c => c.String(nullable: false, maxLength: 120, unicode: false));
            AddColumn("dbo.QueryColumns", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AddColumn("dbo.Reports", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.Restrictions", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AddColumn("dbo.RoleFeatures", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.RuleConditionFields", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AddColumn("dbo.ScreenFields", "ScreenCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AddColumn("dbo.ScreenFields", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AddColumn("dbo.ScreenModifications", "ScreenCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AddColumn("dbo.SLAHeaders", "SearchFields", c => c.String(maxLength: 1000));
            AddColumn("dbo.Translations", "TextCodeCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
            CreateIndex("dbo.Tickets", "SupportMailboxId");
            CreateIndex("dbo.ObjectFields", "FieldCode", unique: true);
            CreateIndex("dbo.Features", "FeatureUniqeCode", unique: true);
            CreateIndex("dbo.BIReports", "LastRunByUserId");
            AddForeignKey("dbo.Tickets", "SupportMailboxId", "dbo.SupportMailboxes", "Id");
            AddForeignKey("dbo.BIReports", "LastRunByUserId", "dbo.Users", "Id");
            DropColumn("dbo.ARInvoices", "DateForVATInterest");
            DropColumn("dbo.ARInvoices", "BillToGLAccountId");
            DropTable("dbo.AirlineAreas");
            DropTable("dbo.AirlineAreasPorts");
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
