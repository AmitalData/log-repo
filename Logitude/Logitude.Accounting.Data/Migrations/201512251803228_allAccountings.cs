namespace Logitude.Accounting.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class allAccountings : DbMigration
    {
        public override void Up()
        {
           
            CreateTable(
                "dbo.AccountingEntities",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 5, unicode: false),
                        LocalName = c.String(maxLength: 30),
                        EnglishName = c.String(nullable: false, maxLength: 60, unicode: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.AccountingPeriods",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        PeriodTypeCode = c.String(nullable: false, maxLength: 1, unicode: false),
                        OpenMonth = c.Int(nullable: false),
                        ClosedMonth = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PeriodTypes", t => t.PeriodTypeCode)
                .Index(t => t.PeriodTypeCode);
            
            CreateTable(
                "dbo.PeriodTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 1, unicode: false),
                        EnglishName = c.String(nullable: false, maxLength: 30, unicode: false),
                        LocalName = c.String(maxLength: 50),
                        SearchFields = c.String(maxLength: 200),
                        Inactive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.AutomaticReconcileMethods",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 15, unicode: false),
                        AutomaticReconcile1 = c.String(nullable: false, maxLength: 15, unicode: false),
                        AutomaticReconcile2 = c.String(maxLength: 15, unicode: false),
                        AutomaticReconcile3 = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AutomaticReconciles", t => t.AutomaticReconcile1)
                .ForeignKey("dbo.AutomaticReconciles", t => t.AutomaticReconcile2)
                .ForeignKey("dbo.AutomaticReconciles", t => t.AutomaticReconcile3)
                .Index(t => t.AutomaticReconcile1)
                .Index(t => t.AutomaticReconcile2)
                .Index(t => t.AutomaticReconcile3);
            
            CreateTable(
                "dbo.AutomaticReconciles",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 15, unicode: false),
                        EnglishName = c.String(nullable: false, maxLength: 60, unicode: false),
                        LocalName = c.String(maxLength: 60),
                        SearchFields = c.String(maxLength: 200),
                        Inactive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.ChartOfAccounts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 5, unicode: false),
                        LocalName = c.String(maxLength: 30),
                        EnglishName = c.String(nullable: false, maxLength: 60, unicode: false),
                        ParentId = c.String(maxLength: 15, unicode: false),
                        TypeCode = c.String(nullable: false, maxLength: 1, unicode: false),
                        Inactive = c.Boolean(),
                        SearchFields = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartOfAccountsTypes", t => t.TypeCode)
                .ForeignKey("dbo.ChartOfAccounts", t => t.ParentId)
                .Index(t => t.ParentId)
                .Index(t => t.TypeCode);
            
            CreateTable(
                "dbo.ChartOfAccountsTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 1, unicode: false),
                        EnglishName = c.String(nullable: false, maxLength: 60, unicode: false),
                        LocalName = c.String(maxLength: 30),
                        SearchFields = c.String(maxLength: 200),
                        Inactive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.GLAccountBalancesByYear",
                c => new
                    {
                        AccountId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Year = c.Int(nullable: false),
                        CurrencyId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        LocalAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                        ForeignAmount = c.Decimal(precision: 16, scale: 2),
                    })
                .PrimaryKey(t => new { t.AccountId, t.Year, t.CurrencyId })
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .ForeignKey("dbo.GLAccounts", t => t.AccountId)
                .Index(t => t.AccountId)
                .Index(t => t.CurrencyId);
            
           
            
            CreateTable(
                "dbo.GLAccounts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        InternalNumber = c.String(maxLength: 15, unicode: false),
                        AccountTypeCode = c.String(maxLength: 1, unicode: false),
                        DisplayNumber = c.String(nullable: false, maxLength: 15, unicode: false),
                        LocalName = c.String(maxLength: 200),
                        EnglishName = c.String(nullable: false, maxLength: 60, unicode: false),
                        SearchFields = c.String(maxLength: 4000),
                        IsMultiCurrency = c.Boolean(),
                        CurrencyId = c.String(maxLength: 15, unicode: false),
                        RevenueExpenseType = c.String(nullable: false, maxLength: 1, unicode: false),
                        IsControlAccount = c.Boolean(),
                        ChartOfAccountsId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Inactive = c.Boolean(),
                        ChartOfAccountsTypeCode = c.String(nullable: false, maxLength: 1, unicode: false),
                        ReconcileMethodCode = c.String(nullable: false, maxLength: 15, unicode: false),
                        ControlAccountId = c.String(maxLength: 15, unicode: false),
                        AutomaticReconcileId = c.String(maxLength: 15, unicode: false),
                        PreviousEnglishName = c.String(maxLength: 60, unicode: false),
                        PreviousEnglishNameChangeDate = c.DateTime(),
                        PreviousLocalName = c.String(maxLength: 200),
                        PreviousLocalNameChangeDate = c.DateTime(),
                        PreviousNumber = c.String(maxLength: 15, unicode: false),
                        PreviousNumberChangeDate = c.DateTime(),
                        PreviousChartOfAccountsId = c.String(maxLength: 15, unicode: false),
                        PreviousChartOfAccountsChangeDate = c.DateTime(),
                        ClientId = c.String(maxLength: 15, unicode: false),
                        VendorId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AutomaticReconcileMethods", t => t.AutomaticReconcileId)
                .ForeignKey("dbo.ChartOfAccounts", t => t.ChartOfAccountsId)
                .ForeignKey("dbo.ChartOfAccountsTypes", t => t.ChartOfAccountsTypeCode)
                .ForeignKey("dbo.Customers", t => t.ClientId)
                .ForeignKey("dbo.GLAccounts", t => t.ControlAccountId)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .ForeignKey("dbo.GLAccountTypes", t => t.AccountTypeCode)
                .ForeignKey("dbo.ChartOfAccounts", t => t.PreviousChartOfAccountsId)
                .ForeignKey("dbo.ReconcileMethods", t => t.ReconcileMethodCode)
                .ForeignKey("dbo.RevenueExpenseTypes", t => t.RevenueExpenseType)
                .ForeignKey("dbo.Vendors", t => t.VendorId)
                .Index(t => t.AccountTypeCode)
                .Index(t => t.CurrencyId)
                .Index(t => t.RevenueExpenseType)
                .Index(t => t.ChartOfAccountsId)
                .Index(t => t.ChartOfAccountsTypeCode)
                .Index(t => t.ReconcileMethodCode)
                .Index(t => t.ControlAccountId)
                .Index(t => t.AutomaticReconcileId)
                .Index(t => t.PreviousChartOfAccountsId)
                .Index(t => t.ClientId)
                .Index(t => t.VendorId);
            
           
            
           
            
            CreateTable(
                "dbo.GLAccountTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 1, unicode: false),
                        LocalName = c.String(maxLength: 30),
                        EnglishName = c.String(nullable: false, maxLength: 60, unicode: false),
                        Inactive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.ReconcileMethods",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 15, unicode: false),
                        EnglishName = c.String(nullable: false, maxLength: 30, unicode: false),
                        LocalName = c.String(maxLength: 60),
                        SearchFields = c.String(maxLength: 200),
                        Inactive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.RevenueExpenseTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 1, unicode: false),
                        LocalName = c.String(maxLength: 30),
                        EnglishName = c.String(nullable: false, maxLength: 60, unicode: false),
                        Inactive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.GLAccountTotalsByMonth",
                c => new
                    {
                        AccountId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        CurrencyId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        LocalAmountDebit = c.Decimal(nullable: false, precision: 16, scale: 2),
                        LocalAmountCredit = c.Decimal(nullable: false, precision: 16, scale: 2),
                        ForeignAmountDebit = c.Decimal(precision: 16, scale: 2),
                        ForeignAmountCredit = c.Decimal(precision: 16, scale: 2),
                    })
                .PrimaryKey(t => new { t.AccountId, t.Year, t.Month, t.CurrencyId })
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .ForeignKey("dbo.GLAccounts", t => t.AccountId)
                .Index(t => t.AccountId)
                .Index(t => t.CurrencyId);
            
            CreateTable(
                "dbo.JournalActionTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 1, unicode: false),
                        LocalName = c.String(maxLength: 30),
                        EnglishName = c.String(nullable: false, maxLength: 60, unicode: false),
                        SearchFields = c.String(maxLength: 200),
                        Inactive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JournalLines",
                c => new
                    {
                        JournalId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Line = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        ActionCode = c.String(maxLength: 15, unicode: false),
                        DebitControlAccountId = c.String(maxLength: 15, unicode: false),
                        DebitAccountId = c.String(maxLength: 15, unicode: false),
                        CreditControlAccountId = c.String(maxLength: 15, unicode: false),
                        CreditAccountId = c.String(maxLength: 15, unicode: false),
                        DocumentDate = c.DateTime(nullable: false),
                        AccountingDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(),
                        LocalAmount = c.Decimal(precision: 16, scale: 2),
                        CurrencyId = c.String(maxLength: 15, unicode: false),
                        ForeignAmount = c.Decimal(precision: 16, scale: 2),
                        ExchangeRate = c.Decimal(precision: 16, scale: 5),
                        Reference1 = c.String(maxLength: 30, unicode: false),
                        Reference2 = c.String(maxLength: 30, unicode: false),
                        Reference3 = c.String(maxLength: 30, unicode: false),
                        Notes = c.String(maxLength: 60),
                    })
                .PrimaryKey(t => new { t.JournalId, t.Line })
                .ForeignKey("dbo.GLAccounts", t => t.CreditAccountId)
                .ForeignKey("dbo.GLAccounts", t => t.CreditControlAccountId)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .ForeignKey("dbo.GLAccounts", t => t.DebitAccountId)
                .ForeignKey("dbo.GLAccounts", t => t.DebitControlAccountId)
                .ForeignKey("dbo.Journals", t => t.JournalId)
                .ForeignKey("dbo.JournalActionTypes", t => t.ActionCode)
                .Index(t => t.JournalId)
                .Index(t => t.ActionCode)
                .Index(t => t.DebitControlAccountId)
                .Index(t => t.DebitAccountId)
                .Index(t => t.CreditControlAccountId)
                .Index(t => t.CreditAccountId)
                .Index(t => t.CurrencyId);
            
            CreateTable(
                "dbo.Journals",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        JournalNumber = c.String(maxLength: 15, unicode: false),
                        CreateDate = c.DateTime(nullable: false),
                        AccountingDate = c.DateTime(nullable: false),
                        TypeCode = c.String(nullable: false, maxLength: 3, unicode: false),
                        StatusCode = c.String(nullable: false, maxLength: 3, unicode: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        AccountingEntityCode = c.String(nullable: false, maxLength: 5, unicode: false),
                        AccountingEntityId = c.String(maxLength: 15, unicode: false),
                        ExternalNo = c.String(maxLength: 30, unicode: false),
                        UpdateDate = c.DateTime(),
                        UpdatedByUserId = c.String(maxLength: 15, unicode: false),
                        ApproveDate = c.DateTime(),
                        ApprovedByUserId = c.String(maxLength: 15, unicode: false),
                        SearchFields = c.String(maxLength: 1000),
                        AccountingEntityReference = c.String(maxLength: 30, unicode: false),
                        OriginalJournalId = c.String(maxLength: 15, unicode: false),
                        VoidedByUserId = c.String(maxLength: 15, unicode: false),
                        VoidDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccountingEntities", t => t.AccountingEntityCode)
                .ForeignKey("dbo.Users", t => t.ApprovedByUserId)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.JournalStatusTypes", t => t.StatusCode)
                .ForeignKey("dbo.JournalTypes", t => t.TypeCode)
                .ForeignKey("dbo.Journals", t => t.OriginalJournalId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .ForeignKey("dbo.Users", t => t.VoidedByUserId)
                .Index(t => t.TypeCode)
                .Index(t => t.StatusCode)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.AccountingEntityCode)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.ApprovedByUserId)
                .Index(t => t.OriginalJournalId)
                .Index(t => t.VoidedByUserId);
            
            CreateTable(
                "dbo.JournalStatusTypes",
                c => new
                    {
                        JournalStatusID = c.String(nullable: false, maxLength: 3, unicode: false),
                        EnglishName = c.String(nullable: false, maxLength: 20, unicode: false),
                        LocalName = c.String(maxLength: 10),
                        SearchFields = c.String(maxLength: 200),
                        Inactive = c.Boolean(),
                    })
                .PrimaryKey(t => t.JournalStatusID);
            
            CreateTable(
                "dbo.JournalTypes",
                c => new
                    {
                        JournalTypeID = c.String(nullable: false, maxLength: 3, unicode: false),
                        EnglishName = c.String(nullable: false, maxLength: 60, unicode: false),
                        LocalName = c.String(maxLength: 30),
                        SearchFields = c.String(maxLength: 200),
                        Inactive = c.Boolean(),
                    })
                .PrimaryKey(t => t.JournalTypeID);
            
            CreateTable(
                "dbo.LedgerTransactions",
                c => new
                    {
                        JournalId = c.String(nullable: false, maxLength: 15, unicode: false),
                        JournalLineNumber = c.Int(nullable: false),
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        ControlAccountId = c.String(maxLength: 15, unicode: false),
                        AccountId = c.String(nullable: false, maxLength: 15, unicode: false),
                        AccountingDate = c.DateTime(nullable: false),
                        DocumentDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        LocalAmountDebit = c.Decimal(nullable: false, precision: 16, scale: 2),
                        LocalAmountCredit = c.Decimal(nullable: false, precision: 16, scale: 2),
                        CurrencyId = c.String(maxLength: 15, unicode: false),
                        ForeignAmountDebit = c.Decimal(nullable: false, precision: 16, scale: 2),
                        ForeignAmountCredit = c.Decimal(nullable: false, precision: 16, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 16, scale: 5),
                        Reference1 = c.String(maxLength: 30, unicode: false),
                        Reference2 = c.String(maxLength: 30, unicode: false),
                        Reference3 = c.String(maxLength: 30, unicode: false),
                        OpenAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                        OppositeAccountId = c.String(maxLength: 15, unicode: false),
                        SearchFields = c.String(maxLength: 4000),
                        OpenAmountCurrencyId = c.String(maxLength: 15, unicode: false),
                        Notes = c.String(maxLength: 150),
                        AmountToReconcile = c.Decimal(nullable: false, precision: 16, scale: 2),
                        Mark = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GLAccounts", t => t.AccountId)
                .ForeignKey("dbo.GLAccounts", t => t.ControlAccountId)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .ForeignKey("dbo.JournalLines", t => new { t.JournalId, t.JournalLineNumber })
                .ForeignKey("dbo.Currencies", t => t.OpenAmountCurrencyId)
                .ForeignKey("dbo.GLAccounts", t => t.OppositeAccountId)
                .Index(t => new { t.JournalId, t.JournalLineNumber })
                .Index(t => t.ControlAccountId)
                .Index(t => t.AccountId)
                .Index(t => t.CurrencyId)
                .Index(t => t.OppositeAccountId)
                .Index(t => t.OpenAmountCurrencyId);
            
            CreateTable(
                "dbo.ReconciliationLines",
                c => new
                    {
                        ReconciliationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Line = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        CurrencyId = c.String(nullable: false, maxLength: 15, unicode: false),
                        TransactionId = c.String(nullable: false, maxLength: 15, unicode: false),
                        ReconciliationAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                        IsPartial = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.ReconciliationId, t.Line })
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .ForeignKey("dbo.LedgerTransactions", t => t.TransactionId)
                .ForeignKey("dbo.Reconciliations", t => t.ReconciliationId)
                .Index(t => t.ReconciliationId)
                .Index(t => t.CurrencyId)
                .Index(t => t.TransactionId);
            
            CreateTable(
                "dbo.Reconciliations",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        AccountId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Number = c.String(maxLength: 15, unicode: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SearchFields = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GLAccounts", t => t.AccountId)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .Index(t => t.AccountId)
                .Index(t => t.CreatedByUserId);
            
            CreateTable(
                "dbo.TestEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        EnglishName = c.String(nullable: false, maxLength: 60, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
           
            
            
          
            
            
            
           
            
         
            
          
           
            
            
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserPermittedProducts", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserPermittedProducts", "ProductTypeCode", "dbo.ProductTypes");
            DropForeignKey("dbo.QuotePackages", "QuoteId", "dbo.Quotes");
            DropForeignKey("dbo.QuotePackages", "PackageTypeId", "dbo.PackageTypes");
            DropForeignKey("dbo.CustomerCompetitorProducts", "ProductTypeCode", "dbo.ProductTypes");
            DropForeignKey("dbo.CustomerCompetitorProducts", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CustomerCompetitorProducts", "CompetitorId", "dbo.Competitors");
            DropForeignKey("dbo.CustomerCompetitors", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CustomerCompetitors", "CompetitorId", "dbo.Competitors");
            DropForeignKey("dbo.Competitors", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.CustomerProductLocationActualDatas", "ProductTypeCode", "dbo.ProductTypes");
            DropForeignKey("dbo.CustomerProductLocationActualDatas", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CustomerProductLocationActualDatas", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.CustomerProductLocations", "ProductTypeCode", "dbo.ProductTypes");
            DropForeignKey("dbo.CustomerProductLocations", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CustomerProductLocations", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.CustomerProductActualDatas", "ProductTypeCode", "dbo.ProductTypes");
            DropForeignKey("dbo.CustomerProductActualDatas", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CustomerProducts", "ProductTypeCode", "dbo.ProductTypes");
            DropForeignKey("dbo.CustomerProducts", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.UserPermittedBranches", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserPermittedBranches", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.CommunicationLogSteps", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.CommunicationLogSteps", "Status", "dbo.CommunicationStatusTypes");
            DropForeignKey("dbo.CommunicationLogSteps", "CommunicationLogId", "dbo.CommunicationLogs");
            DropForeignKey("dbo.ContactLoginLogs", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.Reports", "ReportGroupId", "dbo.ReportGroups");
            DropForeignKey("dbo.Reports", "ReportDocumentId", "dbo.Documents");
            DropForeignKey("dbo.Reports", "FeatureId", "dbo.Features");
            DropForeignKey("dbo.ChargeTypeAccountings", "VatTypeId", "dbo.VatTypes");
            DropForeignKey("dbo.ChargeTypeAccountings", "ChargeTypeId", "dbo.ChargesTypes");
            DropForeignKey("dbo.VatTypePercentages", "VatTypeId", "dbo.VatTypes");
            DropForeignKey("dbo.UserLoginLogs", "UserId", "dbo.Users");
            DropForeignKey("dbo.Translations", "TranslationHeaderCode", "dbo.TranslationHeaders");
            DropForeignKey("dbo.Translations", "TranslatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Translations", "TextCodeId", "dbo.TextCodes");
            DropForeignKey("dbo.TraceEvents", "UserId", "dbo.Users");
            DropForeignKey("dbo.TraceEvents", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.TraceEvents", "EventTypeId", "dbo.EventTypes");
            DropForeignKey("dbo.TipsVisibilities", "UserId", "dbo.Users");
            DropForeignKey("dbo.TipsVisibilities", "TipCode", "dbo.Tips");
            DropForeignKey("dbo.TermsofUseSignatures", "TermsofUseVersion", "dbo.TermsofUses");
            DropForeignKey("dbo.TermsofUseSignatures", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.TenantSettings", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.TarrifSteps", "TarrifHeaderId", "dbo.TarrifHeaders");
            DropForeignKey("dbo.TarrifFromToes", "TarrifHeaderId", "dbo.TarrifHeaders");
            DropForeignKey("dbo.TarrifFromToes", "TarrifFromToTypeCode", "dbo.TarrifFromToTypes");
            DropForeignKey("dbo.TarrifFromToes", "PortId", "dbo.Ports");
            DropForeignKey("dbo.TarrifFromToes", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.TarrifCharges", "TarrifHeaderId", "dbo.TarrifHeaders");
            DropForeignKey("dbo.TarrifHeaders", "TarrifTypeCode", "dbo.TarrifTypes");
            DropForeignKey("dbo.TarrifHeaders", "CardId", "dbo.Cards");
            DropForeignKey("dbo.TarrifCharges", "MeasurementId", "dbo.Measurements");
            DropForeignKey("dbo.TarrifCharges", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.TarrifCharges", "ChargesTypeId", "dbo.ChargesTypes");
            DropForeignKey("dbo.ShipmentReceivables", "UpdateByUserId", "dbo.Users");
            DropForeignKey("dbo.ShipmentReceivables", "ShipmentReceivableLineStatusCode", "dbo.ShipmentReceivableLineStatus");
            DropForeignKey("dbo.ShipmentReceivables", "ShipmentId", "dbo.Shipments");
            DropForeignKey("dbo.ShipmentReceivables", "PrepaidCollectId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.ShipmentReceivables", "MeasurementId", "dbo.Measurements");
            DropForeignKey("dbo.ShipmentReceivables", "IATACodeId", "dbo.IATACodes");
            DropForeignKey("dbo.ShipmentReceivables", "DueTypeCode", "dbo.DueTypes");
            DropForeignKey("dbo.ShipmentReceivables", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ShipmentReceivables", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.ShipmentReceivables", "ChargesTypeId", "dbo.ChargesTypes");
            DropForeignKey("dbo.ShipmentReceivables", "ARInvoiceLineId", "dbo.ARInvoiceLines");
            DropForeignKey("dbo.ShipmentReceivables", "ARInvoiceId", "dbo.ARInvoices");
            DropForeignKey("dbo.ShipmentPickUpDeliveryPackages", "ShipmentPickUpDeliveryId", "dbo.ShipmentPickUpDeliveries");
            DropForeignKey("dbo.ShipmentPickUpDeliveryPackages", "PackageTypeId", "dbo.PackageTypes");
            DropForeignKey("dbo.ShipmentPickUpDeliveries", "ToPortId", "dbo.Ports");
            DropForeignKey("dbo.ShipmentPickUpDeliveries", "ToPartnerCardId", "dbo.Cards");
            DropForeignKey("dbo.ShipmentPickUpDeliveries", "ToAddressId", "dbo.Addresses");
            DropForeignKey("dbo.ShipmentPickUpDeliveries", "ToAddressCountryId", "dbo.Countries");
            DropForeignKey("dbo.ShipmentPickUpDeliveries", "ShipmentId", "dbo.Shipments");
            DropForeignKey("dbo.ShipmentPickUpDeliveries", "PickUpDeliveryTypeCode", "dbo.PickUpDeliveryTypes");
            DropForeignKey("dbo.ShipmentPickUpDeliveries", "PickUpDeliveryFromTypeCode", "dbo.PickUpDeliveryFromToTypes");
            DropForeignKey("dbo.ShipmentPickUpDeliveries", "PickUpDeliveryToTypeCode", "dbo.PickUpDeliveryFromToTypes");
            DropForeignKey("dbo.ShipmentPickUpDeliveries", "FromPortId", "dbo.Ports");
            DropForeignKey("dbo.ShipmentPickUpDeliveries", "FromPartnerCardId", "dbo.Cards");
            DropForeignKey("dbo.ShipmentPickUpDeliveries", "FromAddressId", "dbo.Addresses");
            DropForeignKey("dbo.ShipmentPickUpDeliveries", "FromAddressCountryId", "dbo.Countries");
            DropForeignKey("dbo.ShipmentPickUpDeliveries", "EmptyContainerPartnerId", "dbo.Cards");
            DropForeignKey("dbo.ShipmentPickUpDeliveries", "CarrierId", "dbo.Cards");
            DropForeignKey("dbo.ShipmentPayables", "VendorId", "dbo.Cards");
            DropForeignKey("dbo.ShipmentPayables", "UpdateByUserId", "dbo.Users");
            DropForeignKey("dbo.ShipmentPayables", "ShipmentPayableParentId", "dbo.ShipmentPayables");
            DropForeignKey("dbo.ShipmentPayables", "ShipmentPayableLineStatusCode", "dbo.ShipmentPayableLineStatus");
            DropForeignKey("dbo.ShipmentPayables", "ShipmentPayableAmountTypeCode", "dbo.ShipmentPayableAmountTypes");
            DropForeignKey("dbo.ShipmentPayables", "ShipmentId", "dbo.Shipments");
            DropForeignKey("dbo.ShipmentPayables", "PrepaidCollectId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.ShipmentPayables", "MeasurementId", "dbo.Measurements");
            DropForeignKey("dbo.ShipmentPayables", "IATACodeId", "dbo.IATACodes");
            DropForeignKey("dbo.ShipmentPayables", "DueTypeCode", "dbo.DueTypes");
            DropForeignKey("dbo.ShipmentPayables", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ShipmentPayables", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.ShipmentPayables", "CorrectionByUserId", "dbo.Users");
            DropForeignKey("dbo.ShipmentPayables", "ChargesTypeId", "dbo.ChargesTypes");
            DropForeignKey("dbo.ShipmentOrderPackages", "ShipmentId", "dbo.Shipments");
            DropForeignKey("dbo.ShipmentOrderPackages", "PackageTypeId", "dbo.PackageTypes");
            DropForeignKey("dbo.ShipmentCarrierStatuses", "ToPortId", "dbo.Ports");
            DropForeignKey("dbo.ShipmentCarrierStatuses", "ShipmentId", "dbo.Shipments");
            DropForeignKey("dbo.ShipmentCarrierStatuses", "Location", "dbo.Ports");
            DropForeignKey("dbo.ShipmentCarrierStatuses", "FromPortId", "dbo.Ports");
            DropForeignKey("dbo.ShipmentCarrierStatuses", "Status", "dbo.AWBStatus");
            DropForeignKey("dbo.ShipmentAWBPrintOnlies", "ShipmentId", "dbo.Shipments");
            DropForeignKey("dbo.ShipmentAWBPrintOnlies", "PrepaidCollectId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.ShipmentAWBPrintOnlies", "MeasurementId", "dbo.Measurements");
            DropForeignKey("dbo.ShipmentAWBPrintOnlies", "IATACodeId", "dbo.IATACodes");
            DropForeignKey("dbo.ShipmentAWBPrintOnlies", "DueTypeCode", "dbo.DueTypes");
            DropForeignKey("dbo.ShipmentAWBPrintOnlies", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.SharedLogisticsUpdates", "Status", "dbo.SharedLogisticsUpdateStatus");
            DropForeignKey("dbo.SharedLogisticsUpdates", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.SharedLogisticsUpdates", "HandledByUserId", "dbo.Users");
            DropForeignKey("dbo.SharedLogisticsUpdates", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.ScreenModifications", "ScreenId", "dbo.Screens");
            DropForeignKey("dbo.ScreenFields", "ScreenId", "dbo.Screens");
            DropForeignKey("dbo.ScreenFields", "ObjectFieldId", "dbo.ObjectFields");
            DropForeignKey("dbo.RuleConditionFields", "ObjectTableRuleId", "dbo.ObjectTableRules");
            DropForeignKey("dbo.RuleConditionFields", "ObjectFieldId", "dbo.ObjectFields");
            DropForeignKey("dbo.RoleFeatures", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RoleFeatures", "FeatureAccessLevelCode", "dbo.FeatureAccessLevels");
            DropForeignKey("dbo.RoleFeatures", "FeatureId", "dbo.Features");
            DropForeignKey("dbo.Restrictions", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.Restrictions", "ObjectFieldId", "dbo.ObjectFields");
            DropForeignKey("dbo.Restrictions", "ContactTenantId", "dbo.ContactTenants");
            DropForeignKey("dbo.RatesTables", "ForeignCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.RatesTables", "BaseCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.QuotePriceSteps", "QuoteChargeId", "dbo.QuoteCharges");
            DropForeignKey("dbo.QuotePriceSteps", "QuoteId", "dbo.Quotes");
            DropForeignKey("dbo.QuoteCharges", "VendorId", "dbo.Cards");
            DropForeignKey("dbo.QuoteCharges", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.QuoteCharges", "SaleMeasurementId", "dbo.Measurements");
            DropForeignKey("dbo.QuoteCharges", "QuoteId", "dbo.Quotes");
            DropForeignKey("dbo.QuoteCharges", "MarkUpTypeCode", "dbo.MarkUpTypes");
            DropForeignKey("dbo.QuoteCharges", "SaleCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.QuoteCharges", "CostMeasurementId", "dbo.Measurements");
            DropForeignKey("dbo.QuoteCharges", "CostCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.QuoteCharges", "ContainerType5MarkUpTypeCode", "dbo.MarkUpTypes");
            DropForeignKey("dbo.QuoteCharges", "ContainerType4MarkUpTypeCode", "dbo.MarkUpTypes");
            DropForeignKey("dbo.QuoteCharges", "ContainerType3MarkUpTypeCode", "dbo.MarkUpTypes");
            DropForeignKey("dbo.QuoteCharges", "ContainerType2MarkUpTypeCode", "dbo.MarkUpTypes");
            DropForeignKey("dbo.QuoteCharges", "ContainerType1MarkUpTypeCode", "dbo.MarkUpTypes");
            DropForeignKey("dbo.QuoteCharges", "ChargesTypeId", "dbo.ChargesTypes");
            DropForeignKey("dbo.QueryColumns", "UserId", "dbo.Users");
            DropForeignKey("dbo.QueryColumns", "QueryId", "dbo.Queries");
            DropForeignKey("dbo.QueryColumns", "ObjectFieldId", "dbo.ObjectFields");
            DropForeignKey("dbo.PackageFeatures", "PackageCode", "dbo.Packages");
            DropForeignKey("dbo.PackageFeatures", "FeatureId", "dbo.Features");
            DropForeignKey("dbo.ObjectTableTabs", "TabNameTextCodeId", "dbo.TextCodes");
            DropForeignKey("dbo.ObjectTableTabs", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.ObjectTableTabs", "FeatureId", "dbo.Features");
            DropForeignKey("dbo.ObjectTableRuleFields", "RuleNotificationTypeCode", "dbo.RuleNotificationTypes");
            DropForeignKey("dbo.ObjectTableRuleFields", "ObjectTableRuleId", "dbo.ObjectTableRules");
            DropForeignKey("dbo.ObjectTableRules", "TriggerTypeCode", "dbo.TriggerTypes");
            DropForeignKey("dbo.ObjectTableRules", "RuleTypeCode", "dbo.RuleTypes");
            DropForeignKey("dbo.ObjectTableRules", "RuleNotificationTypeCode", "dbo.RuleNotificationTypes");
            DropForeignKey("dbo.ObjectTableRules", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.ObjectTableRules", "TriggerFieldId", "dbo.ObjectFields");
            DropForeignKey("dbo.ObjectTableRuleFields", "ObjectFieldId", "dbo.ObjectFields");
            DropForeignKey("dbo.ObjectTableHelperControls", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.ObjectTableHelperControls", "FeatureId", "dbo.Features");
            DropForeignKey("dbo.ObjectFieldValidations", "ObjectFieldId", "dbo.ObjectFields");
            DropForeignKey("dbo.ObjectFieldModifications", "ObjectFieldId", "dbo.ObjectFields");
            DropForeignKey("dbo.MenusTables", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.MenusTables", "MenuTypeCode", "dbo.MenuTypes");
            DropForeignKey("dbo.MenusTables", "FeatureId", "dbo.Features");
            DropForeignKey("dbo.MenusTables", "CategoryTypeCode", "dbo.CategoryTypes");
            DropForeignKey("dbo.MenuButtonGroups", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.MenuButtons", "LabelTextCodeId", "dbo.TextCodes");
            DropForeignKey("dbo.MenuButtons", "ParentMenuButtonId", "dbo.MenuButtons");
            DropForeignKey("dbo.MenuButtons", "MenuButtonGroupId", "dbo.MenuButtonGroups");
            DropForeignKey("dbo.MenuButtons", "FeatureId", "dbo.Features");
            DropForeignKey("dbo.MAWBStacks", "AssignedToId", "dbo.Cards");
            DropForeignKey("dbo.MAWBStacks", "AirlineId", "dbo.Airlines");
            DropForeignKey("dbo.InsideShipmentPackages", "ShipmentPackageId", "dbo.ShipmentPackages");
            DropForeignKey("dbo.ShipmentPackages", "ShipmentId", "dbo.Shipments");
            DropForeignKey("dbo.ShipmentPackages", "PackageTypeId", "dbo.PackageTypes");
            DropForeignKey("dbo.ShipmentPackages", "CommodityId", "dbo.ShipmentCommodities");
            DropForeignKey("dbo.ShipmentCommodities", "ShipmentId", "dbo.Shipments");
            DropForeignKey("dbo.ShipmentCommodities", "RateClassCode", "dbo.RateClasses");
            DropForeignKey("dbo.InsideShipmentPackages", "PackageTypeId", "dbo.PackageTypes");
            DropForeignKey("dbo.FormCustomFields1", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.FormCustomFields1", "DocumentTypeId", "dbo.DocumentTypes");
            DropForeignKey("dbo.FollowUps", "ShipmentId", "dbo.Shipments");
            DropForeignKey("dbo.Shipments", "VolumeUnitCode", "dbo.VolumeUnits");
            DropForeignKey("dbo.Shipments", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Shipments", "TransportModeId", "dbo.TransportModes");
            DropForeignKey("dbo.Shipments", "ToPortId", "dbo.Ports");
            DropForeignKey("dbo.Shipments", "SpecialServicesTypeId", "dbo.SpecialServicesTypes");
            DropForeignKey("dbo.Shipments", "ShipperNotExporterContactId", "dbo.Contacts");
            DropForeignKey("dbo.Shipments", "ShipperNotExporterId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "ShipperNotExporterAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Shipments", "ShipperContactId", "dbo.Contacts");
            DropForeignKey("dbo.Shipments", "ShipperId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "ShipperAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Shipments", "ShipmentTypeId", "dbo.ShipmentTypes");
            DropForeignKey("dbo.Shipments", "ShipmentReceivableStatusCode", "dbo.ShipmentReceivableStatus");
            DropForeignKey("dbo.Shipments", "ShipmentPayableStatusCode", "dbo.ShipmentPayableStatus");
            DropForeignKey("dbo.ShipmentMasterDatas", "Transshipment3VesselId", "dbo.Vessels");
            DropForeignKey("dbo.ShipmentMasterDatas", "Transshipment3ToPortId", "dbo.Ports");
            DropForeignKey("dbo.ShipmentMasterDatas", "Transshipment3FromPortId", "dbo.Ports");
            DropForeignKey("dbo.ShipmentMasterDatas", "Transshipment3CarrierId", "dbo.Cards");
            DropForeignKey("dbo.ShipmentMasterDatas", "Transshipment2VesselId", "dbo.Vessels");
            DropForeignKey("dbo.ShipmentMasterDatas", "Transshipment2ToPortId", "dbo.Ports");
            DropForeignKey("dbo.ShipmentMasterDatas", "Transshipment2FromPortId", "dbo.Ports");
            DropForeignKey("dbo.ShipmentMasterDatas", "Transshipment2CarrierId", "dbo.Cards");
            DropForeignKey("dbo.ShipmentMasterDatas", "Transshipment1VesselId", "dbo.Vessels");
            DropForeignKey("dbo.ShipmentMasterDatas", "Transshipment1ToPortId", "dbo.Ports");
            DropForeignKey("dbo.ShipmentMasterDatas", "Transshipment1FromPortId", "dbo.Ports");
            DropForeignKey("dbo.ShipmentMasterDatas", "Transshipment1CarrierId", "dbo.Cards");
            DropForeignKey("dbo.ShipmentMasterDatas", "MainCarriageToPartnerId", "dbo.Cards");
            DropForeignKey("dbo.ShipmentMasterDatas", "MainCarriageToAddressId", "dbo.Addresses");
            DropForeignKey("dbo.ShipmentMasterDatas", "Id", "dbo.Shipments");
            DropForeignKey("dbo.ShipmentMasterDatas", "ManifestStatusCode", "dbo.ManifestStatus");
            DropForeignKey("dbo.ShipmentMasterDatas", "MainCarriageVesselId", "dbo.Vessels");
            DropForeignKey("dbo.ShipmentMasterDatas", "MainCarriageToPortId", "dbo.Ports");
            DropForeignKey("dbo.ShipmentMasterDatas", "MainCarriageFromPortId", "dbo.Ports");
            DropForeignKey("dbo.ShipmentMasterDatas", "MainCarriageFinalDestinationPortId", "dbo.Ports");
            DropForeignKey("dbo.ShipmentMasterDatas", "MainCarriageCarrierId", "dbo.Cards");
            DropForeignKey("dbo.ShipmentMasterDatas", "InterlineId", "dbo.Cards");
            DropForeignKey("dbo.ShipmentMasterDatas", "FWBStatusCode", "dbo.FWBStatus");
            DropForeignKey("dbo.ShipmentMasterDatas", "MainCarriageFromPartnerId", "dbo.Cards");
            DropForeignKey("dbo.ShipmentMasterDatas", "MainCarriageFromAddressId", "dbo.Addresses");
            DropForeignKey("dbo.ShipmentMasterDatas", "StatusId", "dbo.EntityStatus");
            DropForeignKey("dbo.ShipmentMasterDatas", "CargonautFWBStatusCode", "dbo.FWBStatus");
            DropForeignKey("dbo.ShipmentMasterDatas", "AWBPrintingSecurityStatusId", "dbo.AWBSpecialHandlingCodes");
            DropForeignKey("dbo.Shipments", "ShipmentLevelCode", "dbo.ShipmentLevels");
            DropForeignKey("dbo.Shipments", "ShipmentCustomerTypeCode", "dbo.ShipmentCustomerTypes");
            DropForeignKey("dbo.ShipmentComputedFields", "Id", "dbo.Shipments");
            DropForeignKey("dbo.Shipments", "SalesmanUserId", "dbo.Users");
            DropForeignKey("dbo.Shipments", "ProfitCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Shipments", "PreCarriageVesselId", "dbo.Vessels");
            DropForeignKey("dbo.Shipments", "PreCarriageTransportModeId", "dbo.TransportModes");
            DropForeignKey("dbo.Shipments", "PreCarriageToPortId", "dbo.Ports");
            DropForeignKey("dbo.Shipments", "PreCarriageFromPortId", "dbo.Ports");
            DropForeignKey("dbo.Shipments", "PreCarriageCarrierId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "OtherPrepaidCollectId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Shipments", "OnCarriageVesselId", "dbo.Vessels");
            DropForeignKey("dbo.Shipments", "OnCarriageTransportModeId", "dbo.TransportModes");
            DropForeignKey("dbo.Shipments", "OnCarriageToPortId", "dbo.Ports");
            DropForeignKey("dbo.Shipments", "OnCarriageFromPortId", "dbo.Ports");
            DropForeignKey("dbo.Shipments", "OnCarriageCarrierId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "Notify2ContactId", "dbo.Contacts");
            DropForeignKey("dbo.Shipments", "Notify2Id", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "Notify2AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Shipments", "Notify1ContactId", "dbo.Contacts");
            DropForeignKey("dbo.Shipments", "Notify1Id", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "Notify1AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Shipments", "NominatedHandlingPartyId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "NextLegCode", "dbo.NextLegs");
            DropForeignKey("dbo.Shipments", "MoveTypeId", "dbo.MoveTypes");
            DropForeignKey("dbo.MoveTypes", "TransportModeId", "dbo.TransportModes");
            DropForeignKey("dbo.Shipments", "LastSentByUserId", "dbo.Users");
            DropForeignKey("dbo.Shipments", "IssuingCarrierAgentId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "IssuingCarrierAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Shipments", "IncotermId", "dbo.Incoterms");
            DropForeignKey("dbo.Shipments", "ForwarderPartnerId", "dbo.HybridPartners");
            DropForeignKey("dbo.HybridPartners", "LogoId", "dbo.ImageDetails");
            DropForeignKey("dbo.Shipments", "GrossWeightUnitCode", "dbo.WeightUnits");
            DropForeignKey("dbo.Shipments", "FromPortId", "dbo.Ports");
            DropForeignKey("dbo.Shipments", "FreightPrepaidCollectId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Shipments", "FreightLocationId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "FreightForwarderContactId", "dbo.Contacts");
            DropForeignKey("dbo.Shipments", "FreightForwarderId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "FreightForwarderAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Shipments", "FreelancerContactId", "dbo.Contacts");
            DropForeignKey("dbo.Shipments", "FreelancerId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "FreelancerAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Shipments", "FHLStatusCode", "dbo.FHLStatus");
            DropForeignKey("dbo.Shipments", "StatusId", "dbo.EntityStatus");
            DropForeignKey("dbo.Shipments", "DirectionId", "dbo.Directions");
            DropForeignKey("dbo.Shipments", "DimensionsUnitCode", "dbo.DimensionsUnits");
            DropForeignKey("dbo.Shipments", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Shipments", "CustomerContactId", "dbo.Contacts");
            DropForeignKey("dbo.Shipments", "CustomerId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "CustomerAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Shipments", "CustomClearancePointContactId", "dbo.Contacts");
            DropForeignKey("dbo.Shipments", "CustomClearancePointAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Shipments", "CustomClearancePointId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "CustomAgentImportContactId", "dbo.Contacts");
            DropForeignKey("dbo.Shipments", "CustomAgentImportId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "CustomAgentImportAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Shipments", "CustomAgentExportContactId", "dbo.Contacts");
            DropForeignKey("dbo.Shipments", "CustomAgentExportId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "CustomAgentExportAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Shipments", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Shipments", "CountryForStatisticsId", "dbo.Countries");
            DropForeignKey("dbo.Shipments", "ConsolidatorContactId", "dbo.Contacts");
            DropForeignKey("dbo.Shipments", "ConsolidatorId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "ConsolidatorAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Shipments", "ConsigneeNotImporterContactId", "dbo.Contacts");
            DropForeignKey("dbo.Shipments", "ConsigneeNotImporterId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "ConsigneeNotImporterAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Shipments", "ConsigneeContactId", "dbo.Contacts");
            DropForeignKey("dbo.Shipments", "ConsigneeId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "ConsigneeAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Shipments", "ComputedStatusId", "dbo.EntityStatus");
            DropForeignKey("dbo.Shipments", "ColoaderContactId", "dbo.Contacts");
            DropForeignKey("dbo.Shipments", "ColoaderAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Shipments", "ColoaderId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "ChargeableWeightUnitCode", "dbo.WeightUnits");
            DropForeignKey("dbo.Shipments", "CarrierLastStatusCode", "dbo.AWBStatus");
            DropForeignKey("dbo.Shipments", "CargonautFHLStatusCode", "dbo.FHLStatus");
            DropForeignKey("dbo.Shipments", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Shipments", "AWBSpecialHandlingCodeId9", "dbo.AWBSpecialHandlingCodes");
            DropForeignKey("dbo.Shipments", "AWBSpecialHandlingCodeId8", "dbo.AWBSpecialHandlingCodes");
            DropForeignKey("dbo.Shipments", "AWBSpecialHandlingCodeId7", "dbo.AWBSpecialHandlingCodes");
            DropForeignKey("dbo.Shipments", "AWBSpecialHandlingCodeId6", "dbo.AWBSpecialHandlingCodes");
            DropForeignKey("dbo.Shipments", "AWBSpecialHandlingCodeId5", "dbo.AWBSpecialHandlingCodes");
            DropForeignKey("dbo.Shipments", "AWBSpecialHandlingCodeId4", "dbo.AWBSpecialHandlingCodes");
            DropForeignKey("dbo.Shipments", "AWBSpecialHandlingCodeId3", "dbo.AWBSpecialHandlingCodes");
            DropForeignKey("dbo.Shipments", "AWBSpecialHandlingCodeId2", "dbo.AWBSpecialHandlingCodes");
            DropForeignKey("dbo.Shipments", "AWBSpecialHandlingCodeId1", "dbo.AWBSpecialHandlingCodes");
            DropForeignKey("dbo.Shipments", "AWBCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Shipments", "AWBChargesCodeCode", "dbo.AWBChargesCodes");
            DropForeignKey("dbo.Shipments", "AgentContactId", "dbo.Contacts");
            DropForeignKey("dbo.Shipments", "AgentId", "dbo.Cards");
            DropForeignKey("dbo.Shipments", "AgentAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Shipments", "AccountManagerUserId", "dbo.Users");
            DropForeignKey("dbo.Shipments", "AccountingInformationIdentifierCode6", "dbo.AccountingInformationIdentifiers");
            DropForeignKey("dbo.Shipments", "AccountingInformationIdentifierCode5", "dbo.AccountingInformationIdentifiers");
            DropForeignKey("dbo.Shipments", "AccountingInformationIdentifierCode4", "dbo.AccountingInformationIdentifiers");
            DropForeignKey("dbo.Shipments", "AccountingInformationIdentifierCode3", "dbo.AccountingInformationIdentifiers");
            DropForeignKey("dbo.Shipments", "AccountingInformationIdentifierCode2", "dbo.AccountingInformationIdentifiers");
            DropForeignKey("dbo.Shipments", "AccountingInformationIdentifierCode1", "dbo.AccountingInformationIdentifiers");
            DropForeignKey("dbo.FollowUps", "QuoteId", "dbo.Quotes");
            DropForeignKey("dbo.Quotes", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Quotes", "TransportModeId", "dbo.TransportModes");
            DropForeignKey("dbo.Quotes", "ToPortId", "dbo.Ports");
            DropForeignKey("dbo.Quotes", "ToPartnerId", "dbo.Cards");
            DropForeignKey("dbo.Quotes", "ToPartnerAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Quotes", "ToAddressCountryId", "dbo.Countries");
            DropForeignKey("dbo.Quotes", "StageId", "dbo.QuoteStages");
            DropForeignKey("dbo.QuoteStages", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Quotes", "ShipperContactId", "dbo.Contacts");
            DropForeignKey("dbo.Quotes", "ShipperId", "dbo.Cards");
            DropForeignKey("dbo.Quotes", "ShipmentTypeId", "dbo.ShipmentTypes");
            DropForeignKey("dbo.ShipmentTypes", "TransportModeId", "dbo.TransportModes");
            DropForeignKey("dbo.Quotes", "SalesmanUserId", "dbo.Users");
            DropForeignKey("dbo.Quotes", "SaleCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Quotes", "RatingCode", "dbo.QuoteRatings");
            DropForeignKey("dbo.Quotes", "QuoteTypeCode", "dbo.QuoteTypes");
            DropForeignKey("dbo.Quotes", "QuoteTemplateId", "dbo.QuoteTemplates");
            DropForeignKey("dbo.QuoteTemplates", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.QuoteTemplates", "TemplateTypeCode", "dbo.QuoteTypes");
            DropForeignKey("dbo.QuoteTemplates", "QuoteTemplateSettingId", "dbo.QuoteTemplateSettings");
            DropForeignKey("dbo.QuoteTemplateSettings", "TotalsPackagesValueDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "TotalsPackagesLabelDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "GroupByPackagesValueDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "GroupByPackagesLabelDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "TotalsContainsersValueDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "TotalsContainsersLabelDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "GroupByContainsersValueDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "GroupByContainsersLabelDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "PricingPackagesTitleDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "PricingContainsersTitleDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "PageHeaderArea3ImageDetailId", "dbo.ImageDetails");
            DropForeignKey("dbo.QuoteTemplateSettings", "PageHeaderArea3FreeTextDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "PageHeaderArea2ImageDetailId", "dbo.ImageDetails");
            DropForeignKey("dbo.QuoteTemplateSettings", "PageHeaderArea2FreeTextDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "PageHeaderArea1ImageDetailId", "dbo.ImageDetails");
            DropForeignKey("dbo.QuoteTemplateSettings", "PageHeaderArea1FreeTextDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "PageFooterArea3ImageDetailId", "dbo.ImageDetails");
            DropForeignKey("dbo.QuoteTemplateSettings", "PageFooterArea3FreeTextDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "PageFooterArea2ImageDetailId", "dbo.ImageDetails");
            DropForeignKey("dbo.QuoteTemplateSettings", "PageFooterArea2FreeTextDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "PageFooterArea1ImageDetailId", "dbo.ImageDetails");
            DropForeignKey("dbo.QuoteTemplateSettings", "PageFooterArea1FreeTextDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "PackagesTableDesignId", "dbo.QuoteTemplateTableDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "HeaderTableDesignId", "dbo.QuoteTemplateTableDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "DetailsTitleDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "DetailsTableDesignId", "dbo.QuoteTemplateTableDesigns");
            DropForeignKey("dbo.QuoteTemplateSettings", "ContainserTableDesignId", "dbo.QuoteTemplateTableDesigns");
            DropForeignKey("dbo.QuoteTemplateTableDesigns", "LinesDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateTableDesigns", "HeaderDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateTableDesigns", "GroupByDesignId", "dbo.QuoteTemplateTextDesigns");
            DropForeignKey("dbo.QuoteTemplateTableDesigns", "BorderTypeCode", "dbo.BorderTypes");
            DropForeignKey("dbo.QuoteTemplates", "OriginalQuoteTemplateId", "dbo.QuoteTemplates");
            DropForeignKey("dbo.QuoteTemplates", "HeaderDocId", "dbo.Documents");
            DropForeignKey("dbo.QuoteTemplates", "FooterDocId", "dbo.Documents");
            DropForeignKey("dbo.QuoteTemplates", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Quotes", "QuoteCustomerTypeCode", "dbo.QuoteCustomerTypes");
            DropForeignKey("dbo.Quotes", "QuoteClosingReasonCode", "dbo.QuoteClosingReasons");
            DropForeignKey("dbo.Quotes", "FromAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Quotes", "PackageType5Id", "dbo.PackageTypes");
            DropForeignKey("dbo.Quotes", "PackageType4Id", "dbo.PackageTypes");
            DropForeignKey("dbo.Quotes", "PackageType3Id", "dbo.PackageTypes");
            DropForeignKey("dbo.Quotes", "PackageType2Id", "dbo.PackageTypes");
            DropForeignKey("dbo.Quotes", "PackageType1Id", "dbo.PackageTypes");
            DropForeignKey("dbo.PackageTypes", "MeasurementId", "dbo.Measurements");
            DropForeignKey("dbo.Quotes", "MainCarriageCarrierId", "dbo.Cards");
            DropForeignKey("dbo.Quotes", "IncotermId", "dbo.Incoterms");
            DropForeignKey("dbo.Incoterms", "OtherCharges", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Incoterms", "Freight", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Quotes", "FromPortId", "dbo.Ports");
            DropForeignKey("dbo.Ports", "StateId", "dbo.States");
            DropForeignKey("dbo.Ports", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Quotes", "FromPartnerId", "dbo.Cards");
            DropForeignKey("dbo.Quotes", "FromPartnerAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Quotes", "FromAddressCountryId", "dbo.Countries");
            DropForeignKey("dbo.Quotes", "FreelancerContactId", "dbo.Contacts");
            DropForeignKey("dbo.Quotes", "FreelancerId", "dbo.Cards");
            DropForeignKey("dbo.Quotes", "FreelancerAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Quotes", "DirectionId", "dbo.Directions");
            DropForeignKey("dbo.Quotes", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Quotes", "ToAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Quotes", "CustomerContactId", "dbo.Contacts");
            DropForeignKey("dbo.Quotes", "CustomerId", "dbo.Cards");
            DropForeignKey("dbo.Quotes", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Quotes", "ConsigneeContactId", "dbo.Contacts");
            DropForeignKey("dbo.Quotes", "ConsigneeId", "dbo.Cards");
            DropForeignKey("dbo.Quotes", "BusinessUnitId", "dbo.BusinessUnits");
            DropForeignKey("dbo.Quotes", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Quotes", "AgentContactId", "dbo.Contacts");
            DropForeignKey("dbo.Quotes", "AgentId", "dbo.Cards");
            DropForeignKey("dbo.Quotes", "AgentAddressId", "dbo.Addresses");
            DropForeignKey("dbo.FollowUps", "OwnerUserId", "dbo.Users");
            DropForeignKey("dbo.FollowUps", "InternalDocumentId", "dbo.DocumentOuts");
            DropForeignKey("dbo.FollowUps", "DocumentsFilingId", "dbo.DocumentsFilings");
            DropForeignKey("dbo.FollowUps", "EventTypeId", "dbo.EventTypes");
            DropForeignKey("dbo.EventTypes", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.EventTypes", "EventTypeCategoryCode", "dbo.EventTypeCategories");
            DropForeignKey("dbo.EventTypes", "EntityStatusId", "dbo.EntityStatus");
            DropForeignKey("dbo.EventTypes", "CustomerRoleId", "dbo.Roles");
            DropForeignKey("dbo.EventTypes", "AgentRoleId", "dbo.Roles");
            DropForeignKey("dbo.EntityStatus", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.EntityLastUpdates", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.EntityLastUpdates", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.EntityLastActivities", "UserId", "dbo.Users");
            DropForeignKey("dbo.EntityLastActivities", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.EntityLastActivities", "ActivityTypeCode", "dbo.EntityLastActivityTypes");
            DropForeignKey("dbo.DocumentTypeTemplates", "OriginalTemplateId", "dbo.DocumentTypeTemplates");
            DropForeignKey("dbo.DocumentTypeTemplates", "LastUpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.DocumentTypeTemplates", "DocumentTypeId", "dbo.DocumentTypes");
            DropForeignKey("dbo.DocumentTypeCustomFields1", "FieldDataTypeCode", "dbo.FieldDataTypes");
            DropForeignKey("dbo.DocumentTypeCustomFields1", "DocumentTypeId", "dbo.DocumentTypes");
            DropForeignKey("dbo.DocumentOutCopies", "LastPrintedByUserId", "dbo.Users");
            DropForeignKey("dbo.DocumentOutCopies", "DocumentTypeCopyId", "dbo.DocumentTypeCopies");
            DropForeignKey("dbo.DocumentTypeCopies", "DocumentTypeId", "dbo.DocumentTypes");
            DropForeignKey("dbo.DocumentOutCopies", "DocumentOutId", "dbo.DocumentOuts");
            DropForeignKey("dbo.DocumentOutCopies", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.CustomTables", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.CounterStats", "CounterId", "dbo.Counters");
            DropForeignKey("dbo.CounterDefinitions", "CounterId", "dbo.Counters");
            DropForeignKey("dbo.Counters", "ChangedByUserId", "dbo.Users");
            DropForeignKey("dbo.Counters", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.ContactTenantRoleSet", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.ContactTenantRoleSet", "ContactTenantId", "dbo.ContactTenants");
            DropForeignKey("dbo.ContactTenants", "TenantId", "dbo.Tenants");
            DropForeignKey("dbo.ContactTenants", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.CommunicationAttachments", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.CommunicationAttachments", "CommunicationLogId", "dbo.CommunicationLogs");
            DropForeignKey("dbo.CommunicationLogs", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.CommunicationLogs", "DocumentOutId", "dbo.DocumentOuts");
            DropForeignKey("dbo.CommunicationLogs", "DocumentsFilingId", "dbo.DocumentsFilings");
            DropForeignKey("dbo.DocumentsFilings", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.DocumentsFilings", "ReceivedByUserId", "dbo.Users");
            DropForeignKey("dbo.DocumentsFilings", "OwnerId", "dbo.Users");
            DropForeignKey("dbo.DocumentsFilings", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.DocumentsFilings", "FolderId", "dbo.DocumentFolders");
            DropForeignKey("dbo.DocumentFolders", "ParentFolderId", "dbo.DocumentFolders");
            DropForeignKey("dbo.DocumentsFilings", "DocumentTypeId", "dbo.DocumentTypes");
            DropForeignKey("dbo.DocumentTypes", "TemplateFormatCode", "dbo.TemplateFormats");
            DropForeignKey("dbo.DocumentTypes", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.DocumentTypes", "DocumentTypeCategoryCode", "dbo.DocumentTypeCategories");
            DropForeignKey("dbo.DocumentTypes", "DocumentsDataProviderCode", "dbo.DocumentsDataProviders");
            DropForeignKey("dbo.DocumentTypes", "CustomerRoleId", "dbo.Roles");
            DropForeignKey("dbo.DocumentTypes", "AgentRoleId", "dbo.Roles");
            DropForeignKey("dbo.Roles", "RoleTypeCode", "dbo.RoleTypes");
            DropForeignKey("dbo.DocumentsFilings", "StatusCode", "dbo.DocumentStatus");
            DropForeignKey("dbo.DocumentOuts", "XamlDocumentId", "dbo.Documents");
            DropForeignKey("dbo.DocumentOuts", "IssuedByUserId", "dbo.Users");
            DropForeignKey("dbo.DocumentOuts", "Id", "dbo.DocumentsFilings");
            DropForeignKey("dbo.DocumentsFilings", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.DocumentsFilings", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.DocumentsFilings", "DeletedByUserId", "dbo.Users");
            DropForeignKey("dbo.DocumentsFilings", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.DocumentsFilings", "ChildObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.DocumentsFilings", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.CommunicationLogs", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.Documents", "SmallDocumentId", "dbo.SmallDocuments");
            DropForeignKey("dbo.CommunicationLogs", "Tenant", "dbo.Tenants");
            DropForeignKey("dbo.CommunicationLogs", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.CommunicationLogs", "CommunicationStatusTypeCode", "dbo.CommunicationStatusTypes");
            DropForeignKey("dbo.CommunicationLogs", "CommunicationLogTypeCode", "dbo.CommunicationLogTypes");
            DropForeignKey("dbo.CardContacts", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.CardContacts", "CardId", "dbo.Cards");
            DropForeignKey("dbo.AWBSpecialHandlingCodes", "AirlineId", "dbo.Cards");
            DropForeignKey("dbo.ARInvoiceTotalVATs", "VatTypeId", "dbo.VatTypes");
            DropForeignKey("dbo.ARInvoiceTotalVATs", "ARInvoiceId", "dbo.ARInvoices");
            DropForeignKey("dbo.ARInvoicePayments", "ForeignCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ARInvoicePayments", "ARPaymentId", "dbo.ARPayments");
            DropForeignKey("dbo.ARPayments", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.ARPayments", "StatusCode", "dbo.ARPaymentStatus");
            DropForeignKey("dbo.ARPayments", "PrintByUserId", "dbo.Users");
            DropForeignKey("dbo.ARPayments", "PaymentCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ARPayments", "LocalCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ARPayments", "DebitAccountId", "dbo.Accounts1");
            DropForeignKey("dbo.ARPayments", "CreditCardTypeId", "dbo.CreditCardTypes");
            DropForeignKey("dbo.ARPayments", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.ARPayments", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.ARPayments", "BillToId", "dbo.Cards");
            DropForeignKey("dbo.ARPayments", "BillToAddressId", "dbo.Addresses");
            DropForeignKey("dbo.ARPayments", "ARPaymentMethodCode", "dbo.ARPaymentMethods");
            DropForeignKey("dbo.ARPayments", "ARAccountId", "dbo.Accounts1");
            DropForeignKey("dbo.ARInvoicePayments", "ARInvoiceId", "dbo.ARInvoices");
            DropForeignKey("dbo.ARInvoiceLines", "VatTypeId", "dbo.VatTypes");
            DropForeignKey("dbo.ARInvoiceLines", "MeasurementId", "dbo.Measurements");
            DropForeignKey("dbo.ARInvoiceLines", "ForiegnCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ARInvoiceLines", "ChargesTypeId", "dbo.ChargesTypes");
            DropForeignKey("dbo.ARInvoiceLines", "ARInvoiceId", "dbo.ARInvoices");
            DropForeignKey("dbo.ARInvoiceEntities", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.ARInvoiceEntities", "ARInvoiceId", "dbo.ARInvoices");
            DropForeignKey("dbo.ARInvoices", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.ARInvoices", "TransferStatusCode", "dbo.ARInvoiceTransferStatus");
            DropForeignKey("dbo.ARInvoices", "StatusCode", "dbo.ARInvoiceStatus");
            DropForeignKey("dbo.ARInvoices", "ProfitCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ARInvoices", "PrintByUserId", "dbo.Users");
            DropForeignKey("dbo.ARInvoices", "PrepaidCollectId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.ARInvoices", "PaymentTermId", "dbo.PaymentTerms");
            DropForeignKey("dbo.ARInvoices", "LocalCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ARInvoices", "IssuedByUserId", "dbo.Users");
            DropForeignKey("dbo.ARInvoices", "InvoiceCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ARInvoices", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.ARInvoices", "CancelledByARInvoiceId", "dbo.ARInvoices");
            DropForeignKey("dbo.ARInvoices", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.ARInvoices", "BillToAddressId", "dbo.Addresses");
            DropForeignKey("dbo.ARInvoices", "BillToId", "dbo.Cards");
            DropForeignKey("dbo.ARInvoices", "ARInvoiceTypeCode", "dbo.ARInvoiceTypes");
            DropForeignKey("dbo.APInvoiceTotalVATs", "VatTypeId", "dbo.VatTypes");
            DropForeignKey("dbo.APInvoiceTotalVATs", "APInvoiceId", "dbo.APInvoices");
            DropForeignKey("dbo.APInvoicePayments", "ForeignCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.APInvoicePayments", "APPaymentId", "dbo.APPayments");
            DropForeignKey("dbo.APPayments", "VendorId", "dbo.Cards");
            DropForeignKey("dbo.APPayments", "VendorAddressId", "dbo.Addresses");
            DropForeignKey("dbo.APPayments", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.APPayments", "StatusCode", "dbo.APPaymentStatus");
            DropForeignKey("dbo.APPayments", "PrintedByUserId", "dbo.Users");
            DropForeignKey("dbo.APPayments", "PaymentMethodCode", "dbo.APPaymentMethods");
            DropForeignKey("dbo.APPayments", "PaymentCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.APPayments", "LocalCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.APPayments", "CreditCardTypeId", "dbo.CreditCardTypes");
            DropForeignKey("dbo.APPayments", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.APPayments", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.APInvoicePayments", "APInvoiceId", "dbo.APInvoices");
            DropForeignKey("dbo.APInvoiceLines", "VatTypeId", "dbo.VatTypes");
            DropForeignKey("dbo.APInvoiceLines", "ForiegnCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.APInvoiceLines", "ChargesTypeId", "dbo.ChargesTypes");
            DropForeignKey("dbo.ChargesTypes", "VatTypeId", "dbo.VatTypes");
            DropForeignKey("dbo.ChargesTypes", "ReceivableAccountId", "dbo.Accounts1");
            DropForeignKey("dbo.ChargesTypes", "PayableAccountId", "dbo.Accounts1");
            DropForeignKey("dbo.ChargesTypes", "MeasurementId", "dbo.Measurements");
            DropForeignKey("dbo.ChargesTypes", "IATACodeId", "dbo.IATACodes");
            DropForeignKey("dbo.ChargesTypes", "DueTypeCode", "dbo.DueTypes");
            DropForeignKey("dbo.ChargesTypes", "ContainerMeasurementId", "dbo.Measurements");
            DropForeignKey("dbo.ChargesTypes", "ChargesGroupCode", "dbo.ChargesGroups");
            DropForeignKey("dbo.IATACodes", "Code", "dbo.ChargesGroups");
            DropForeignKey("dbo.IATACodes", "DueTypeCode", "dbo.DueTypes");
            DropForeignKey("dbo.IATACodes", "AirlineId", "dbo.Cards");
            DropForeignKey("dbo.APInvoiceLines", "APInvoiceId", "dbo.APInvoices");
            DropForeignKey("dbo.APInvoiceEntities", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.APInvoiceEntities", "APInvoiceId", "dbo.APInvoices");
            DropForeignKey("dbo.APInvoices", "VendorId", "dbo.Cards");
            DropForeignKey("dbo.APInvoices", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.APInvoices", "TransferStatusCode", "dbo.APInvoiceTransferStatus");
            DropForeignKey("dbo.APInvoices", "StatusCode", "dbo.APInvoiceStatus");
            DropForeignKey("dbo.APInvoices", "ProfitCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.APInvoices", "PaymentTermId", "dbo.PaymentTerms");
            DropForeignKey("dbo.APInvoices", "LocalCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.APInvoices", "InvoiceCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.APInvoices", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.APInvoices", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.AdvancedQueryFilters", "UserId", "dbo.Users");
            DropForeignKey("dbo.AdvancedQueryFilters", "QueryId", "dbo.Queries");
            DropForeignKey("dbo.Queries", "UserId", "dbo.Users");
            DropForeignKey("dbo.Queries", "QueryGroupCode", "dbo.QueryGroups");
            DropForeignKey("dbo.Queries", "OriginalQueryId", "dbo.Queries");
            DropForeignKey("dbo.Queries", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.Queries", "NameTextCodeId", "dbo.TextCodes");
            DropForeignKey("dbo.Queries", "FeatureId", "dbo.Features");
            DropForeignKey("dbo.Features", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.Features", "NameTextCodeId", "dbo.TextCodes");
            DropForeignKey("dbo.Features", "FeatureTypeCode", "dbo.FeatureTypes");
            DropForeignKey("dbo.AdvancedQueryFilters", "ObjectFieldId", "dbo.ObjectFields");
            DropForeignKey("dbo.ObjectFields", "ShortNameTextCodeId", "dbo.TextCodes");
            DropForeignKey("dbo.ObjectFields", "MultiTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.ObjectFields", "LookUpTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.ObjectFields", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.ObjectFields", "ListTextCodeId", "dbo.TextCodes");
            DropForeignKey("dbo.ObjectFields", "HelpTextCodeId", "dbo.TextCodes");
            DropForeignKey("dbo.ObjectFields", "FullNameTextCodeId", "dbo.TextCodes");
            DropForeignKey("dbo.TextCodes", "TextCodeTypeCode", "dbo.TextCodeTypes");
            DropForeignKey("dbo.TextCodes", "SpellCheckedByUserId", "dbo.Users");
            DropForeignKey("dbo.TextCodes", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.ObjectTables", "ObjectTableTypeCode", "dbo.ObjectTableTypes");
            DropForeignKey("dbo.ObjectTables", "NewButtonTextCodeId", "dbo.TextCodes");
            DropForeignKey("dbo.ObjectTables", "MainTipCode", "dbo.Tips");
            DropForeignKey("dbo.Tips", "ShortTextCode", "dbo.TextCodes");
            DropForeignKey("dbo.Tips", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.ObjectTables", "HeaderScreenId", "dbo.Screens");
            DropForeignKey("dbo.Screens", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.ObjectTables", "DescriptionTextCodeId", "dbo.TextCodes");
            DropForeignKey("dbo.ObjectFields", "DataTypeCode", "dbo.FieldDataTypes");
            DropForeignKey("dbo.ObjectFields", "CustomerPermissionTypeCode", "dbo.PermissionTypes");
            DropForeignKey("dbo.ObjectFields", "AgentPermissionTypeCode", "dbo.PermissionTypes");
            DropForeignKey("dbo.Accounts1", "AccountTypeCode", "dbo.AccountTypes");
            DropForeignKey("dbo.AccountingSettings", "Id", "dbo.Tenants");
            DropForeignKey("dbo.Tenants", "VolumeUnitCode", "dbo.VolumeUnits");
            DropForeignKey("dbo.Tenants", "VatUniqueTypeCode", "dbo.VatUniqueTypes");
            DropForeignKey("dbo.Tenants", "VatUniqueCountryId", "dbo.Countries");
            DropForeignKey("dbo.Tenants", "VatMandatoryTypeCode", "dbo.VatMandatoryTypes");
            DropForeignKey("dbo.Tenants", "VatMandatoryCountryId", "dbo.Countries");
            DropForeignKey("dbo.Tenants", "QuoteSaleCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Tenants", "ProfitCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Tenants", "PaymentTermId", "dbo.PaymentTerms");
            DropForeignKey("dbo.Tenants", "PasswordPolicyCode", "dbo.PasswordPolicies");
            DropForeignKey("dbo.Tenants", "OtherChargesCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Tenants", "MasterImportOtherPrepaidCollectId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Tenants", "MasterImportFreightPrepaidCollectId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Tenants", "MasterExportOtherPrepaidCollectId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Tenants", "MasterExportFreightPrepaidCollectId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Tenants", "ImportOtherPrepaidCollectId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Tenants", "ImportFreightPrepaidCollectId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Tenants", "GrossWeightUnitCode", "dbo.WeightUnits");
            DropForeignKey("dbo.Tenants", "FreightCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Tenants", "ExportOtherPrepaidCollectId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Tenants", "ExportFreightPrepaidCollectId", "dbo.PrepaidCollects");
            DropForeignKey("dbo.Tenants", "DimensionsUnitCode", "dbo.DimensionsUnits");
            DropForeignKey("dbo.Tenants", "CustomerId", "dbo.Cards");
            DropForeignKey("dbo.Tenants", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Tenants", "WeightMeasurementUnitCode", "dbo.WeightUnits");
            DropForeignKey("dbo.Tenants", "AgentId", "dbo.Cards");
            DropForeignKey("dbo.Tenants", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Addresses", "StateId", "dbo.States");
            DropForeignKey("dbo.States", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Addresses", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Addresses", "CardId", "dbo.Cards");
            DropForeignKey("dbo.Addresses", "AddressTypeId", "dbo.AddressTypes");
            DropForeignKey("dbo.AccountingSettings", "AccountingSystemCode", "dbo.AccountingSystems");
            DropForeignKey("dbo.ReconciliationLines", "ReconciliationId", "dbo.Reconciliations");
            DropForeignKey("dbo.Reconciliations", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Reconciliations", "AccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.ReconciliationLines", "TransactionId", "dbo.LedgerTransactions");
            DropForeignKey("dbo.ReconciliationLines", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.LedgerTransactions", "OppositeAccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.LedgerTransactions", "OpenAmountCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.LedgerTransactions", new[] { "JournalId", "JournalLineNumber" }, "dbo.JournalLines");
            DropForeignKey("dbo.LedgerTransactions", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.LedgerTransactions", "ControlAccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.LedgerTransactions", "AccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.JournalLines", "ActionCode", "dbo.JournalActionTypes");
            DropForeignKey("dbo.JournalLines", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.Journals", "VoidedByUserId", "dbo.Users");
            DropForeignKey("dbo.Journals", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Journals", "OriginalJournalId", "dbo.Journals");
            DropForeignKey("dbo.Journals", "TypeCode", "dbo.JournalTypes");
            DropForeignKey("dbo.Journals", "StatusCode", "dbo.JournalStatusTypes");
            DropForeignKey("dbo.Journals", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Journals", "ApprovedByUserId", "dbo.Users");
            DropForeignKey("dbo.Journals", "AccountingEntityCode", "dbo.AccountingEntities");
            DropForeignKey("dbo.JournalLines", "DebitControlAccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.JournalLines", "DebitAccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.JournalLines", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.JournalLines", "CreditControlAccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.JournalLines", "CreditAccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.GLAccountTotalsByMonth", "AccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.GLAccountTotalsByMonth", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.GLAccountBalancesByYear", "AccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.GLAccounts", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.GLAccounts", "RevenueExpenseType", "dbo.RevenueExpenseTypes");
            DropForeignKey("dbo.GLAccounts", "ReconcileMethodCode", "dbo.ReconcileMethods");
            DropForeignKey("dbo.GLAccounts", "PreviousChartOfAccountsId", "dbo.ChartOfAccounts");
            DropForeignKey("dbo.GLAccounts", "AccountTypeCode", "dbo.GLAccountTypes");
            DropForeignKey("dbo.GLAccounts", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.GLAccounts", "ControlAccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.GLAccounts", "ClientId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "SalesmanUserId", "dbo.Users");
            DropForeignKey("dbo.Customers", "RegionId", "dbo.Regions");
            DropForeignKey("dbo.Customers", "RankId", "dbo.Ranks");
            DropForeignKey("dbo.Customers", "MediatorId", "dbo.Cards");
            DropForeignKey("dbo.Customers", "LeadSourceId", "dbo.LeadSources");
            DropForeignKey("dbo.Customers", "IndustryId", "dbo.Industries");
            DropForeignKey("dbo.Customers", "FreelancerId", "dbo.Users");
            DropForeignKey("dbo.Customers", "ForwarderId", "dbo.Cards");
            DropForeignKey("dbo.Customers", "CustomsAgentId", "dbo.Cards");
            DropForeignKey("dbo.Customers", "CustomerStatusCode", "dbo.CustomerStatus");
            DropForeignKey("dbo.Customers", "CustomerSizeId", "dbo.CustomerSizes");
            DropForeignKey("dbo.Customers", "CollectorId", "dbo.Users");
            DropForeignKey("dbo.Customers", "ClassifierId", "dbo.Users");
            DropForeignKey("dbo.Customers", "Id", "dbo.Cards");
            DropForeignKey("dbo.Customers", "BillToId", "dbo.Cards");
            DropForeignKey("dbo.Customers", "BeforeDeactiveStatusCode", "dbo.CustomerStatus");
            DropForeignKey("dbo.Customers", "AccountManagerUserId", "dbo.Users");
            DropForeignKey("dbo.UserLastLogins", "Id", "dbo.Users");
            DropForeignKey("dbo.Users", "ProductTypeCode", "dbo.ProductTypes");
            DropForeignKey("dbo.Users", "FreelancerId", "dbo.Cards");
            DropForeignKey("dbo.Warehouses", "Id", "dbo.Cards");
            DropForeignKey("dbo.Vendors", "Id", "dbo.Cards");
            DropForeignKey("dbo.Cards", "VatTypeId", "dbo.VatTypes");
            DropForeignKey("dbo.Cards", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Truckers", "Id", "dbo.Cards");
            DropForeignKey("dbo.ShippingLines", "ShippingAgentId", "dbo.ShippingAgents");
            DropForeignKey("dbo.ShippingLines", "Id", "dbo.Cards");
            DropForeignKey("dbo.ShippingAgents", "Id", "dbo.Cards");
            DropForeignKey("dbo.Cards", "SharedLogisticsInvitationStatusCode", "dbo.SharedLogisticsInvitationStatus");
            DropForeignKey("dbo.Cards", "SalesmanUserId", "dbo.Users");
            DropForeignKey("dbo.Cards", "PrimaryContactId", "dbo.Contacts");
            DropForeignKey("dbo.Cards", "PaymentTermId", "dbo.PaymentTerms");
            DropForeignKey("dbo.Cards", "PartnerTypeId", "dbo.PartnerTypes");
            DropForeignKey("dbo.Cards", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Countries", "GlobalZoneId", "dbo.GlobalZones");
            DropForeignKey("dbo.Cards", "InvoiceCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Cards", "ImageDetailId", "dbo.ImageDetails");
            DropForeignKey("dbo.CustomAgents", "Id", "dbo.Cards");
            DropForeignKey("dbo.Cards", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Cards", "CollectorId", "dbo.Users");
            DropForeignKey("dbo.Cards", "ClassifierId", "dbo.Users");
            DropForeignKey("dbo.Airlines", "Id", "dbo.Cards");
            DropForeignKey("dbo.Agents", "Id", "dbo.Cards");
            DropForeignKey("dbo.Users", "DistributorCode", "dbo.Distributors");
            DropForeignKey("dbo.Users", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Users", "Id", "dbo.Contacts");
            DropForeignKey("dbo.Contacts", "ImageDetailId", "dbo.ImageDetails");
            DropForeignKey("dbo.ContactLastLogins", "Id", "dbo.Contacts");
            DropForeignKey("dbo.Contacts", "ContactDoneMethodCode", "dbo.ContactDoneMethods");
            DropForeignKey("dbo.Contacts", "IndexColor", "dbo.ColorIndexes");
            DropForeignKey("dbo.Users", "BusinessUnitId", "dbo.BusinessUnits");
            DropForeignKey("dbo.Users", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.GLAccounts", "ChartOfAccountsTypeCode", "dbo.ChartOfAccountsTypes");
            DropForeignKey("dbo.GLAccounts", "ChartOfAccountsId", "dbo.ChartOfAccounts");
            DropForeignKey("dbo.GLAccounts", "AutomaticReconcileId", "dbo.AutomaticReconcileMethods");
            DropForeignKey("dbo.GLAccountBalancesByYear", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ChartOfAccounts", "ParentId", "dbo.ChartOfAccounts");
            DropForeignKey("dbo.ChartOfAccounts", "TypeCode", "dbo.ChartOfAccountsTypes");
            DropForeignKey("dbo.AutomaticReconcileMethods", "AutomaticReconcile3", "dbo.AutomaticReconciles");
            DropForeignKey("dbo.AutomaticReconcileMethods", "AutomaticReconcile2", "dbo.AutomaticReconciles");
            DropForeignKey("dbo.AutomaticReconcileMethods", "AutomaticReconcile1", "dbo.AutomaticReconciles");
            DropForeignKey("dbo.AccountingPeriods", "PeriodTypeCode", "dbo.PeriodTypes");
            DropIndex("dbo.UserPermittedProducts", new[] { "ProductTypeCode" });
            DropIndex("dbo.UserPermittedProducts", new[] { "UserId" });
            DropIndex("dbo.QuotePackages", new[] { "QuoteId" });
            DropIndex("dbo.QuotePackages", new[] { "PackageTypeId" });
            DropIndex("dbo.CustomerCompetitorProducts", new[] { "CompetitorId" });
            DropIndex("dbo.CustomerCompetitorProducts", new[] { "CustomerId" });
            DropIndex("dbo.CustomerCompetitorProducts", new[] { "ProductTypeCode" });
            DropIndex("dbo.CustomerCompetitors", new[] { "CompetitorId" });
            DropIndex("dbo.CustomerCompetitors", new[] { "CustomerId" });
            DropIndex("dbo.Competitors", new[] { "AddressId" });
            DropIndex("dbo.CustomerProductLocationActualDatas", new[] { "CountryId" });
            DropIndex("dbo.CustomerProductLocationActualDatas", new[] { "ProductTypeCode" });
            DropIndex("dbo.CustomerProductLocationActualDatas", new[] { "CustomerId" });
            DropIndex("dbo.CustomerProductLocations", new[] { "CountryId" });
            DropIndex("dbo.CustomerProductLocations", new[] { "ProductTypeCode" });
            DropIndex("dbo.CustomerProductLocations", new[] { "CustomerId" });
            DropIndex("dbo.CustomerProductActualDatas", new[] { "ProductTypeCode" });
            DropIndex("dbo.CustomerProductActualDatas", new[] { "CustomerId" });
            DropIndex("dbo.CustomerProducts", new[] { "ProductTypeCode" });
            DropIndex("dbo.CustomerProducts", new[] { "CustomerId" });
            DropIndex("dbo.UserPermittedBranches", new[] { "BranchId" });
            DropIndex("dbo.UserPermittedBranches", new[] { "UserId" });
            DropIndex("dbo.CommunicationLogSteps", new[] { "DocumentId" });
            DropIndex("dbo.CommunicationLogSteps", new[] { "Status" });
            DropIndex("dbo.CommunicationLogSteps", new[] { "CommunicationLogId" });
            DropIndex("dbo.ContactLoginLogs", new[] { "ContactId" });
            DropIndex("dbo.Reports", new[] { "ReportDocumentId" });
            DropIndex("dbo.Reports", new[] { "FeatureId" });
            DropIndex("dbo.Reports", new[] { "ReportGroupId" });
            DropIndex("dbo.ChargeTypeAccountings", new[] { "ChargeTypeId" });
            DropIndex("dbo.ChargeTypeAccountings", new[] { "VatTypeId" });
            DropIndex("dbo.VatTypePercentages", new[] { "VatTypeId" });
            DropIndex("dbo.UserLoginLogs", new[] { "UserId" });
            DropIndex("dbo.Translations", new[] { "TranslatedByUserId" });
            DropIndex("dbo.Translations", new[] { "TextCodeId" });
            DropIndex("dbo.Translations", new[] { "TranslationHeaderCode" });
            DropIndex("dbo.TraceEvents", new[] { "UserId" });
            DropIndex("dbo.TraceEvents", new[] { "ObjectTableId" });
            DropIndex("dbo.TraceEvents", new[] { "EventTypeId" });
            DropIndex("dbo.TipsVisibilities", new[] { "TipCode" });
            DropIndex("dbo.TipsVisibilities", new[] { "UserId" });
            DropIndex("dbo.TermsofUseSignatures", new[] { "TermsofUseVersion" });
            DropIndex("dbo.TermsofUseSignatures", new[] { "ContactId" });
            DropIndex("dbo.TenantSettings", new[] { "ObjectTableId" });
            DropIndex("dbo.TarrifSteps", new[] { "TarrifHeaderId" });
            DropIndex("dbo.TarrifFromToes", new[] { "TarrifFromToTypeCode" });
            DropIndex("dbo.TarrifFromToes", new[] { "CountryId" });
            DropIndex("dbo.TarrifFromToes", new[] { "PortId" });
            DropIndex("dbo.TarrifFromToes", new[] { "TarrifHeaderId" });
            DropIndex("dbo.TarrifHeaders", new[] { "TarrifTypeCode" });
            DropIndex("dbo.TarrifHeaders", new[] { "CardId" });
            DropIndex("dbo.TarrifCharges", new[] { "MeasurementId" });
            DropIndex("dbo.TarrifCharges", new[] { "ChargesTypeId" });
            DropIndex("dbo.TarrifCharges", new[] { "CurrencyId" });
            DropIndex("dbo.TarrifCharges", new[] { "TarrifHeaderId" });
            DropIndex("dbo.ShipmentReceivables", new[] { "IATACodeId" });
            DropIndex("dbo.ShipmentReceivables", new[] { "CreatedByUserId" });
            DropIndex("dbo.ShipmentReceivables", new[] { "ARInvoiceId" });
            DropIndex("dbo.ShipmentReceivables", new[] { "ARInvoiceLineId" });
            DropIndex("dbo.ShipmentReceivables", new[] { "DueTypeCode" });
            DropIndex("dbo.ShipmentReceivables", new[] { "PrepaidCollectId" });
            DropIndex("dbo.ShipmentReceivables", new[] { "UpdateByUserId" });
            DropIndex("dbo.ShipmentReceivables", new[] { "CurrencyId" });
            DropIndex("dbo.ShipmentReceivables", new[] { "MeasurementId" });
            DropIndex("dbo.ShipmentReceivables", new[] { "ShipmentReceivableLineStatusCode" });
            DropIndex("dbo.ShipmentReceivables", new[] { "ChargesTypeId" });
            DropIndex("dbo.ShipmentReceivables", new[] { "ShipmentId" });
            DropIndex("dbo.ShipmentPickUpDeliveryPackages", new[] { "ShipmentPickUpDeliveryId" });
            DropIndex("dbo.ShipmentPickUpDeliveryPackages", new[] { "PackageTypeId" });
            DropIndex("dbo.ShipmentPickUpDeliveries", new[] { "ToAddressCountryId" });
            DropIndex("dbo.ShipmentPickUpDeliveries", new[] { "ToAddressId" });
            DropIndex("dbo.ShipmentPickUpDeliveries", new[] { "ToPartnerCardId" });
            DropIndex("dbo.ShipmentPickUpDeliveries", new[] { "ToPortId" });
            DropIndex("dbo.ShipmentPickUpDeliveries", new[] { "PickUpDeliveryToTypeCode" });
            DropIndex("dbo.ShipmentPickUpDeliveries", new[] { "FromAddressCountryId" });
            DropIndex("dbo.ShipmentPickUpDeliveries", new[] { "FromAddressId" });
            DropIndex("dbo.ShipmentPickUpDeliveries", new[] { "FromPartnerCardId" });
            DropIndex("dbo.ShipmentPickUpDeliveries", new[] { "FromPortId" });
            DropIndex("dbo.ShipmentPickUpDeliveries", new[] { "PickUpDeliveryFromTypeCode" });
            DropIndex("dbo.ShipmentPickUpDeliveries", new[] { "PickUpDeliveryTypeCode" });
            DropIndex("dbo.ShipmentPickUpDeliveries", new[] { "EmptyContainerPartnerId" });
            DropIndex("dbo.ShipmentPickUpDeliveries", new[] { "CarrierId" });
            DropIndex("dbo.ShipmentPickUpDeliveries", new[] { "ShipmentId" });
            DropIndex("dbo.ShipmentPayables", new[] { "IATACodeId" });
            DropIndex("dbo.ShipmentPayables", new[] { "CorrectionByUserId" });
            DropIndex("dbo.ShipmentPayables", new[] { "CreatedByUserId" });
            DropIndex("dbo.ShipmentPayables", new[] { "ShipmentPayableAmountTypeCode" });
            DropIndex("dbo.ShipmentPayables", new[] { "ShipmentPayableParentId" });
            DropIndex("dbo.ShipmentPayables", new[] { "PrepaidCollectId" });
            DropIndex("dbo.ShipmentPayables", new[] { "DueTypeCode" });
            DropIndex("dbo.ShipmentPayables", new[] { "VendorId" });
            DropIndex("dbo.ShipmentPayables", new[] { "UpdateByUserId" });
            DropIndex("dbo.ShipmentPayables", new[] { "CurrencyId" });
            DropIndex("dbo.ShipmentPayables", new[] { "MeasurementId" });
            DropIndex("dbo.ShipmentPayables", new[] { "ShipmentPayableLineStatusCode" });
            DropIndex("dbo.ShipmentPayables", new[] { "ChargesTypeId" });
            DropIndex("dbo.ShipmentPayables", new[] { "ShipmentId" });
            DropIndex("dbo.ShipmentOrderPackages", new[] { "PackageTypeId" });
            DropIndex("dbo.ShipmentOrderPackages", new[] { "ShipmentId" });
            DropIndex("dbo.ShipmentCarrierStatuses", new[] { "Location" });
            DropIndex("dbo.ShipmentCarrierStatuses", new[] { "ToPortId" });
            DropIndex("dbo.ShipmentCarrierStatuses", new[] { "FromPortId" });
            DropIndex("dbo.ShipmentCarrierStatuses", new[] { "Status" });
            DropIndex("dbo.ShipmentCarrierStatuses", new[] { "ShipmentId" });
            DropIndex("dbo.ShipmentAWBPrintOnlies", new[] { "MeasurementId" });
            DropIndex("dbo.ShipmentAWBPrintOnlies", new[] { "IATACodeId" });
            DropIndex("dbo.ShipmentAWBPrintOnlies", new[] { "ShipmentId" });
            DropIndex("dbo.ShipmentAWBPrintOnlies", new[] { "DueTypeCode" });
            DropIndex("dbo.ShipmentAWBPrintOnlies", new[] { "PrepaidCollectId" });
            DropIndex("dbo.ShipmentAWBPrintOnlies", new[] { "CurrencyId" });
            DropIndex("dbo.SharedLogisticsUpdates", new[] { "HandledByUserId" });
            DropIndex("dbo.SharedLogisticsUpdates", new[] { "Status" });
            DropIndex("dbo.SharedLogisticsUpdates", new[] { "DocumentId" });
            DropIndex("dbo.SharedLogisticsUpdates", new[] { "ObjectTableId" });
            DropIndex("dbo.ScreenModifications", new[] { "ScreenId" });
            DropIndex("dbo.ScreenFields", new[] { "ObjectFieldId" });
            DropIndex("dbo.ScreenFields", new[] { "ScreenId" });
            DropIndex("dbo.RuleConditionFields", new[] { "ObjectFieldId" });
            DropIndex("dbo.RuleConditionFields", new[] { "ObjectTableRuleId" });
            DropIndex("dbo.RoleFeatures", new[] { "FeatureAccessLevelCode" });
            DropIndex("dbo.RoleFeatures", new[] { "FeatureId" });
            DropIndex("dbo.RoleFeatures", new[] { "RoleId" });
            DropIndex("dbo.Restrictions", new[] { "ContactTenantId" });
            DropIndex("dbo.Restrictions", new[] { "ObjectFieldId" });
            DropIndex("dbo.Restrictions", new[] { "ObjectTableId" });
            DropIndex("dbo.RatesTables", new[] { "ForeignCurrencyId" });
            DropIndex("dbo.RatesTables", new[] { "BaseCurrencyId" });
            DropIndex("dbo.QuotePriceSteps", new[] { "QuoteChargeId" });
            DropIndex("dbo.QuotePriceSteps", new[] { "QuoteId" });
            DropIndex("dbo.QuoteCharges", new[] { "ContainerType5MarkUpTypeCode" });
            DropIndex("dbo.QuoteCharges", new[] { "ContainerType4MarkUpTypeCode" });
            DropIndex("dbo.QuoteCharges", new[] { "ContainerType3MarkUpTypeCode" });
            DropIndex("dbo.QuoteCharges", new[] { "ContainerType2MarkUpTypeCode" });
            DropIndex("dbo.QuoteCharges", new[] { "ContainerType1MarkUpTypeCode" });
            DropIndex("dbo.QuoteCharges", new[] { "MarkUpTypeCode" });
            DropIndex("dbo.QuoteCharges", new[] { "UpdatedByUserId" });
            DropIndex("dbo.QuoteCharges", new[] { "SaleCurrencyId" });
            DropIndex("dbo.QuoteCharges", new[] { "SaleMeasurementId" });
            DropIndex("dbo.QuoteCharges", new[] { "CostCurrencyId" });
            DropIndex("dbo.QuoteCharges", new[] { "CostMeasurementId" });
            DropIndex("dbo.QuoteCharges", new[] { "VendorId" });
            DropIndex("dbo.QuoteCharges", new[] { "ChargesTypeId" });
            DropIndex("dbo.QuoteCharges", new[] { "QuoteId" });
            DropIndex("dbo.QueryColumns", new[] { "UserId" });
            DropIndex("dbo.QueryColumns", new[] { "ObjectFieldId" });
            DropIndex("dbo.QueryColumns", new[] { "QueryId" });
            DropIndex("dbo.PackageFeatures", new[] { "FeatureId" });
            DropIndex("dbo.PackageFeatures", new[] { "PackageCode" });
            DropIndex("dbo.ObjectTableTabs", new[] { "FeatureId" });
            DropIndex("dbo.ObjectTableTabs", new[] { "TabNameTextCodeId" });
            DropIndex("dbo.ObjectTableTabs", new[] { "ObjectTableId" });
            DropIndex("dbo.ObjectTableRules", new[] { "TriggerFieldId" });
            DropIndex("dbo.ObjectTableRules", new[] { "RuleNotificationTypeCode" });
            DropIndex("dbo.ObjectTableRules", new[] { "TriggerTypeCode" });
            DropIndex("dbo.ObjectTableRules", new[] { "ObjectTableId" });
            DropIndex("dbo.ObjectTableRules", new[] { "RuleTypeCode" });
            DropIndex("dbo.ObjectTableRuleFields", new[] { "RuleNotificationTypeCode" });
            DropIndex("dbo.ObjectTableRuleFields", new[] { "ObjectTableRuleId" });
            DropIndex("dbo.ObjectTableRuleFields", new[] { "ObjectFieldId" });
            DropIndex("dbo.ObjectTableHelperControls", new[] { "FeatureId" });
            DropIndex("dbo.ObjectTableHelperControls", new[] { "ObjectTableId" });
            DropIndex("dbo.ObjectFieldValidations", new[] { "ObjectFieldId" });
            DropIndex("dbo.ObjectFieldModifications", new[] { "ObjectFieldId" });
            DropIndex("dbo.MenusTables", new[] { "FeatureId" });
            DropIndex("dbo.MenusTables", new[] { "ObjectTableId" });
            DropIndex("dbo.MenusTables", new[] { "CategoryTypeCode" });
            DropIndex("dbo.MenusTables", new[] { "MenuTypeCode" });
            DropIndex("dbo.MenuButtons", new[] { "FeatureId" });
            DropIndex("dbo.MenuButtons", new[] { "ParentMenuButtonId" });
            DropIndex("dbo.MenuButtons", new[] { "MenuButtonGroupId" });
            DropIndex("dbo.MenuButtons", new[] { "LabelTextCodeId" });
            DropIndex("dbo.MenuButtonGroups", new[] { "ObjectTableId" });
            DropIndex("dbo.MAWBStacks", new[] { "AssignedToId" });
            DropIndex("dbo.MAWBStacks", new[] { "AirlineId" });
            DropIndex("dbo.ShipmentCommodities", new[] { "RateClassCode" });
            DropIndex("dbo.ShipmentCommodities", new[] { "ShipmentId" });
            DropIndex("dbo.ShipmentPackages", new[] { "CommodityId" });
            DropIndex("dbo.ShipmentPackages", new[] { "ShipmentId" });
            DropIndex("dbo.ShipmentPackages", new[] { "PackageTypeId" });
            DropIndex("dbo.InsideShipmentPackages", new[] { "PackageTypeId" });
            DropIndex("dbo.InsideShipmentPackages", new[] { "ShipmentPackageId" });
            DropIndex("dbo.FormCustomFields1", new[] { "ObjectTableId" });
            DropIndex("dbo.FormCustomFields1", new[] { "DocumentTypeId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "ManifestStatusCode" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "InterlineId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "AWBPrintingSecurityStatusId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "CargonautFWBStatusCode" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "StatusId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "MainCarriageToAddressId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "MainCarriageToPartnerId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "MainCarriageFromAddressId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "MainCarriageFromPartnerId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "FWBStatusCode" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "MainCarriageFinalDestinationPortId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "Transshipment3VesselId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "Transshipment2VesselId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "Transshipment1VesselId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "MainCarriageVesselId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "Transshipment3CarrierId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "Transshipment3ToPortId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "Transshipment3FromPortId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "Transshipment2CarrierId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "Transshipment2ToPortId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "Transshipment2FromPortId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "Transshipment1CarrierId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "Transshipment1ToPortId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "Transshipment1FromPortId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "MainCarriageCarrierId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "MainCarriageToPortId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "MainCarriageFromPortId" });
            DropIndex("dbo.ShipmentMasterDatas", new[] { "Id" });
            DropIndex("dbo.ShipmentComputedFields", new[] { "Id" });
            DropIndex("dbo.MoveTypes", new[] { "TransportModeId" });
            DropIndex("dbo.HybridPartners", new[] { "LogoId" });
            DropIndex("dbo.Shipments", new[] { "LastSentByUserId" });
            DropIndex("dbo.Shipments", new[] { "ForwarderPartnerId" });
            DropIndex("dbo.Shipments", new[] { "CargonautFHLStatusCode" });
            DropIndex("dbo.Shipments", new[] { "AccountingInformationIdentifierCode6" });
            DropIndex("dbo.Shipments", new[] { "AccountingInformationIdentifierCode5" });
            DropIndex("dbo.Shipments", new[] { "AccountingInformationIdentifierCode4" });
            DropIndex("dbo.Shipments", new[] { "AccountingInformationIdentifierCode3" });
            DropIndex("dbo.Shipments", new[] { "AccountingInformationIdentifierCode2" });
            DropIndex("dbo.Shipments", new[] { "AccountingInformationIdentifierCode1" });
            DropIndex("dbo.Shipments", new[] { "NominatedHandlingPartyId" });
            DropIndex("dbo.Shipments", new[] { "SpecialServicesTypeId" });
            DropIndex("dbo.Shipments", new[] { "FreightLocationId" });
            DropIndex("dbo.Shipments", new[] { "ColoaderContactId" });
            DropIndex("dbo.Shipments", new[] { "ColoaderAddressId" });
            DropIndex("dbo.Shipments", new[] { "ColoaderId" });
            DropIndex("dbo.Shipments", new[] { "CustomClearancePointContactId" });
            DropIndex("dbo.Shipments", new[] { "CustomClearancePointAddressId" });
            DropIndex("dbo.Shipments", new[] { "CustomClearancePointId" });
            DropIndex("dbo.Shipments", new[] { "MoveTypeId" });
            DropIndex("dbo.Shipments", new[] { "FHLStatusCode" });
            DropIndex("dbo.Shipments", new[] { "AWBCurrencyId" });
            DropIndex("dbo.Shipments", new[] { "AWBChargesCodeCode" });
            DropIndex("dbo.Shipments", new[] { "OnCarriageVesselId" });
            DropIndex("dbo.Shipments", new[] { "OnCarriageCarrierId" });
            DropIndex("dbo.Shipments", new[] { "OnCarriageToPortId" });
            DropIndex("dbo.Shipments", new[] { "OnCarriageFromPortId" });
            DropIndex("dbo.Shipments", new[] { "OnCarriageTransportModeId" });
            DropIndex("dbo.Shipments", new[] { "PreCarriageVesselId" });
            DropIndex("dbo.Shipments", new[] { "PreCarriageCarrierId" });
            DropIndex("dbo.Shipments", new[] { "PreCarriageToPortId" });
            DropIndex("dbo.Shipments", new[] { "PreCarriageFromPortId" });
            DropIndex("dbo.Shipments", new[] { "PreCarriageTransportModeId" });
            DropIndex("dbo.Shipments", new[] { "ToPortId" });
            DropIndex("dbo.Shipments", new[] { "FromPortId" });
            DropIndex("dbo.Shipments", new[] { "ConsolidatorContactId" });
            DropIndex("dbo.Shipments", new[] { "ConsolidatorAddressId" });
            DropIndex("dbo.Shipments", new[] { "ConsolidatorId" });
            DropIndex("dbo.Shipments", new[] { "FreelancerContactId" });
            DropIndex("dbo.Shipments", new[] { "FreelancerAddressId" });
            DropIndex("dbo.Shipments", new[] { "FreelancerId" });
            DropIndex("dbo.Shipments", new[] { "ConsigneeNotImporterContactId" });
            DropIndex("dbo.Shipments", new[] { "ConsigneeNotImporterAddressId" });
            DropIndex("dbo.Shipments", new[] { "ConsigneeNotImporterId" });
            DropIndex("dbo.Shipments", new[] { "ShipperNotExporterContactId" });
            DropIndex("dbo.Shipments", new[] { "ShipperNotExporterAddressId" });
            DropIndex("dbo.Shipments", new[] { "ShipperNotExporterId" });
            DropIndex("dbo.Shipments", new[] { "Notify2ContactId" });
            DropIndex("dbo.Shipments", new[] { "Notify2AddressId" });
            DropIndex("dbo.Shipments", new[] { "Notify2Id" });
            DropIndex("dbo.Shipments", new[] { "Notify1ContactId" });
            DropIndex("dbo.Shipments", new[] { "Notify1AddressId" });
            DropIndex("dbo.Shipments", new[] { "Notify1Id" });
            DropIndex("dbo.Shipments", new[] { "CustomAgentImportContactId" });
            DropIndex("dbo.Shipments", new[] { "CustomAgentImportAddressId" });
            DropIndex("dbo.Shipments", new[] { "CustomAgentImportId" });
            DropIndex("dbo.Shipments", new[] { "CustomAgentExportContactId" });
            DropIndex("dbo.Shipments", new[] { "CustomAgentExportAddressId" });
            DropIndex("dbo.Shipments", new[] { "CustomAgentExportId" });
            DropIndex("dbo.Shipments", new[] { "AgentContactId" });
            DropIndex("dbo.Shipments", new[] { "AgentAddressId" });
            DropIndex("dbo.Shipments", new[] { "AgentId" });
            DropIndex("dbo.Shipments", new[] { "ConsigneeContactId" });
            DropIndex("dbo.Shipments", new[] { "ConsigneeAddressId" });
            DropIndex("dbo.Shipments", new[] { "ConsigneeId" });
            DropIndex("dbo.Shipments", new[] { "ShipperContactId" });
            DropIndex("dbo.Shipments", new[] { "ShipperAddressId" });
            DropIndex("dbo.Shipments", new[] { "ShipperId" });
            DropIndex("dbo.Shipments", new[] { "FreightForwarderContactId" });
            DropIndex("dbo.Shipments", new[] { "FreightForwarderAddressId" });
            DropIndex("dbo.Shipments", new[] { "FreightForwarderId" });
            DropIndex("dbo.Shipments", new[] { "CustomerContactId" });
            DropIndex("dbo.Shipments", new[] { "CustomerAddressId" });
            DropIndex("dbo.Shipments", new[] { "CustomerId" });
            DropIndex("dbo.Shipments", new[] { "ShipmentCustomerTypeCode" });
            DropIndex("dbo.Shipments", new[] { "IssuingCarrierAddressId" });
            DropIndex("dbo.Shipments", new[] { "IssuingCarrierAgentId" });
            DropIndex("dbo.Shipments", new[] { "AccountManagerUserId" });
            DropIndex("dbo.Shipments", new[] { "ComputedStatusId" });
            DropIndex("dbo.Shipments", new[] { "AWBSpecialHandlingCodeId9" });
            DropIndex("dbo.Shipments", new[] { "AWBSpecialHandlingCodeId8" });
            DropIndex("dbo.Shipments", new[] { "AWBSpecialHandlingCodeId7" });
            DropIndex("dbo.Shipments", new[] { "AWBSpecialHandlingCodeId6" });
            DropIndex("dbo.Shipments", new[] { "AWBSpecialHandlingCodeId5" });
            DropIndex("dbo.Shipments", new[] { "AWBSpecialHandlingCodeId4" });
            DropIndex("dbo.Shipments", new[] { "AWBSpecialHandlingCodeId3" });
            DropIndex("dbo.Shipments", new[] { "AWBSpecialHandlingCodeId2" });
            DropIndex("dbo.Shipments", new[] { "AWBSpecialHandlingCodeId1" });
            DropIndex("dbo.Shipments", new[] { "NextLegCode" });
            DropIndex("dbo.Shipments", new[] { "ShipmentLevelCode" });
            DropIndex("dbo.Shipments", new[] { "ProfitCurrencyId" });
            DropIndex("dbo.Shipments", new[] { "UpdatedByUserId" });
            DropIndex("dbo.Shipments", new[] { "ShipmentPayableStatusCode" });
            DropIndex("dbo.Shipments", new[] { "ShipmentReceivableStatusCode" });
            DropIndex("dbo.Shipments", new[] { "StatusId" });
            DropIndex("dbo.Shipments", new[] { "VolumeUnitCode" });
            DropIndex("dbo.Shipments", new[] { "DimensionsUnitCode" });
            DropIndex("dbo.Shipments", new[] { "ChargeableWeightUnitCode" });
            DropIndex("dbo.Shipments", new[] { "GrossWeightUnitCode" });
            DropIndex("dbo.Shipments", new[] { "OtherPrepaidCollectId" });
            DropIndex("dbo.Shipments", new[] { "FreightPrepaidCollectId" });
            DropIndex("dbo.Shipments", new[] { "DepartmentId" });
            DropIndex("dbo.Shipments", new[] { "CreatedByUserId" });
            DropIndex("dbo.Shipments", new[] { "SalesmanUserId" });
            DropIndex("dbo.Shipments", new[] { "IncotermId" });
            DropIndex("dbo.Shipments", new[] { "BranchId" });
            DropIndex("dbo.Shipments", new[] { "ShipmentTypeId" });
            DropIndex("dbo.Shipments", new[] { "TransportModeId" });
            DropIndex("dbo.Shipments", new[] { "DirectionId" });
            DropIndex("dbo.Shipments", new[] { "CarrierLastStatusCode" });
            DropIndex("dbo.Shipments", new[] { "CountryForStatisticsId" });
            DropIndex("dbo.QuoteStages", new[] { "UpdatedByUserId" });
            DropIndex("dbo.ShipmentTypes", new[] { "TransportModeId" });
            DropIndex("dbo.QuoteTemplateTableDesigns", new[] { "GroupByDesignId" });
            DropIndex("dbo.QuoteTemplateTableDesigns", new[] { "LinesDesignId" });
            DropIndex("dbo.QuoteTemplateTableDesigns", new[] { "HeaderDesignId" });
            DropIndex("dbo.QuoteTemplateTableDesigns", new[] { "BorderTypeCode" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "PricingContainsersTitleDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "PricingPackagesTitleDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "DetailsTitleDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "HeaderTableDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "DetailsTableDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "GroupByContainsersValueDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "GroupByContainsersLabelDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "GroupByPackagesValueDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "GroupByPackagesLabelDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "TotalsContainsersValueDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "TotalsPackagesValueDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "TotalsContainsersLabelDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "TotalsPackagesLabelDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "ContainserTableDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "PackagesTableDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "PageFooterArea3FreeTextDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "PageFooterArea2FreeTextDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "PageFooterArea1FreeTextDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "PageFooterArea3ImageDetailId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "PageFooterArea2ImageDetailId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "PageFooterArea1ImageDetailId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "PageHeaderArea3FreeTextDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "PageHeaderArea2FreeTextDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "PageHeaderArea1FreeTextDesignId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "PageHeaderArea3ImageDetailId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "PageHeaderArea2ImageDetailId" });
            DropIndex("dbo.QuoteTemplateSettings", new[] { "PageHeaderArea1ImageDetailId" });
            DropIndex("dbo.QuoteTemplates", new[] { "TemplateTypeCode" });
            DropIndex("dbo.QuoteTemplates", new[] { "UpdatedByUserId" });
            DropIndex("dbo.QuoteTemplates", new[] { "CreatedByUserId" });
            DropIndex("dbo.QuoteTemplates", new[] { "OriginalQuoteTemplateId" });
            DropIndex("dbo.QuoteTemplates", new[] { "QuoteTemplateSettingId" });
            DropIndex("dbo.QuoteTemplates", new[] { "FooterDocId" });
            DropIndex("dbo.QuoteTemplates", new[] { "HeaderDocId" });
            DropIndex("dbo.PackageTypes", new[] { "MeasurementId" });
            DropIndex("dbo.Incoterms", new[] { "OtherCharges" });
            DropIndex("dbo.Incoterms", new[] { "Freight" });
            DropIndex("dbo.Ports", new[] { "StateId" });
            DropIndex("dbo.Ports", new[] { "CountryId" });
            DropIndex("dbo.Quotes", new[] { "QuoteClosingReasonCode" });
            DropIndex("dbo.Quotes", new[] { "StageId" });
            DropIndex("dbo.Quotes", new[] { "RatingCode" });
            DropIndex("dbo.Quotes", new[] { "BusinessUnitId" });
            DropIndex("dbo.Quotes", new[] { "UpdatedByUserId" });
            DropIndex("dbo.Quotes", new[] { "AgentContactId" });
            DropIndex("dbo.Quotes", new[] { "AgentAddressId" });
            DropIndex("dbo.Quotes", new[] { "AgentId" });
            DropIndex("dbo.Quotes", new[] { "ToPartnerAddressId" });
            DropIndex("dbo.Quotes", new[] { "FromPartnerAddressId" });
            DropIndex("dbo.Quotes", new[] { "ToPartnerId" });
            DropIndex("dbo.Quotes", new[] { "FromPartnerId" });
            DropIndex("dbo.Quotes", new[] { "ToAddressCountryId" });
            DropIndex("dbo.Quotes", new[] { "FromAddressCountryId" });
            DropIndex("dbo.Quotes", new[] { "ToAddressId" });
            DropIndex("dbo.Quotes", new[] { "FromAddressId" });
            DropIndex("dbo.Quotes", new[] { "SaleCurrencyId" });
            DropIndex("dbo.Quotes", new[] { "MainCarriageCarrierId" });
            DropIndex("dbo.Quotes", new[] { "QuoteTypeCode" });
            DropIndex("dbo.Quotes", new[] { "PackageType5Id" });
            DropIndex("dbo.Quotes", new[] { "PackageType4Id" });
            DropIndex("dbo.Quotes", new[] { "PackageType3Id" });
            DropIndex("dbo.Quotes", new[] { "PackageType2Id" });
            DropIndex("dbo.Quotes", new[] { "PackageType1Id" });
            DropIndex("dbo.Quotes", new[] { "BranchId" });
            DropIndex("dbo.Quotes", new[] { "DepartmentId" });
            DropIndex("dbo.Quotes", new[] { "CreatedByUserId" });
            DropIndex("dbo.Quotes", new[] { "SalesmanUserId" });
            DropIndex("dbo.Quotes", new[] { "IncotermId" });
            DropIndex("dbo.Quotes", new[] { "ToPortId" });
            DropIndex("dbo.Quotes", new[] { "FromPortId" });
            DropIndex("dbo.Quotes", new[] { "FreelancerContactId" });
            DropIndex("dbo.Quotes", new[] { "FreelancerAddressId" });
            DropIndex("dbo.Quotes", new[] { "FreelancerId" });
            DropIndex("dbo.Quotes", new[] { "ConsigneeContactId" });
            DropIndex("dbo.Quotes", new[] { "ConsigneeId" });
            DropIndex("dbo.Quotes", new[] { "ShipperContactId" });
            DropIndex("dbo.Quotes", new[] { "ShipperId" });
            DropIndex("dbo.Quotes", new[] { "CustomerContactId" });
            DropIndex("dbo.Quotes", new[] { "CustomerId" });
            DropIndex("dbo.Quotes", new[] { "QuoteCustomerTypeCode" });
            DropIndex("dbo.Quotes", new[] { "QuoteTemplateId" });
            DropIndex("dbo.Quotes", new[] { "ShipmentTypeId" });
            DropIndex("dbo.Quotes", new[] { "TransportModeId" });
            DropIndex("dbo.Quotes", new[] { "DirectionId" });
            DropIndex("dbo.FollowUps", new[] { "OwnerUserId" });
            DropIndex("dbo.FollowUps", new[] { "QuoteId" });
            DropIndex("dbo.FollowUps", new[] { "EventTypeId" });
            DropIndex("dbo.FollowUps", new[] { "InternalDocumentId" });
            DropIndex("dbo.FollowUps", new[] { "DocumentsFilingId" });
            DropIndex("dbo.FollowUps", new[] { "ShipmentId" });
            DropIndex("dbo.EventTypes", new[] { "EventTypeCategoryCode" });
            DropIndex("dbo.EventTypes", new[] { "AgentRoleId" });
            DropIndex("dbo.EventTypes", new[] { "CustomerRoleId" });
            DropIndex("dbo.EventTypes", new[] { "ObjectTableId" });
            DropIndex("dbo.EventTypes", new[] { "EntityStatusId" });
            DropIndex("dbo.EntityStatus", new[] { "ObjectTableId" });
            DropIndex("dbo.EntityLastUpdates", new[] { "UpdatedByUserId" });
            DropIndex("dbo.EntityLastUpdates", new[] { "ObjectTableId" });
            DropIndex("dbo.EntityLastActivities", new[] { "ActivityTypeCode" });
            DropIndex("dbo.EntityLastActivities", new[] { "UserId" });
            DropIndex("dbo.EntityLastActivities", new[] { "ObjectTableId" });
            DropIndex("dbo.DocumentTypeTemplates", new[] { "OriginalTemplateId" });
            DropIndex("dbo.DocumentTypeTemplates", new[] { "DocumentTypeId" });
            DropIndex("dbo.DocumentTypeTemplates", new[] { "LastUpdatedByUserId" });
            DropIndex("dbo.DocumentTypeCustomFields1", new[] { "FieldDataTypeCode" });
            DropIndex("dbo.DocumentTypeCustomFields1", new[] { "DocumentTypeId" });
            DropIndex("dbo.DocumentTypeCopies", new[] { "DocumentTypeId" });
            DropIndex("dbo.DocumentOutCopies", new[] { "LastPrintedByUserId" });
            DropIndex("dbo.DocumentOutCopies", new[] { "DocumentTypeCopyId" });
            DropIndex("dbo.DocumentOutCopies", new[] { "DocumentOutId" });
            DropIndex("dbo.DocumentOutCopies", new[] { "DocumentId" });
            DropIndex("dbo.CustomTables", new[] { "ObjectTableId" });
            DropIndex("dbo.CounterStats", new[] { "CounterId" });
            DropIndex("dbo.Counters", new[] { "ChangedByUserId" });
            DropIndex("dbo.Counters", new[] { "ObjectTableId" });
            DropIndex("dbo.CounterDefinitions", new[] { "CounterId" });
            DropIndex("dbo.ContactTenants", new[] { "ContactId" });
            DropIndex("dbo.ContactTenants", new[] { "TenantId" });
            DropIndex("dbo.ContactTenantRoleSet", new[] { "ContactTenantId" });
            DropIndex("dbo.ContactTenantRoleSet", new[] { "RoleId" });
            DropIndex("dbo.DocumentFolders", new[] { "ParentFolderId" });
            DropIndex("dbo.Roles", new[] { "RoleTypeCode" });
            DropIndex("dbo.DocumentTypes", new[] { "DocumentTypeCategoryCode" });
            DropIndex("dbo.DocumentTypes", new[] { "DocumentsDataProviderCode" });
            DropIndex("dbo.DocumentTypes", new[] { "AgentRoleId" });
            DropIndex("dbo.DocumentTypes", new[] { "CustomerRoleId" });
            DropIndex("dbo.DocumentTypes", new[] { "TemplateFormatCode" });
            DropIndex("dbo.DocumentTypes", new[] { "ObjectTableId" });
            DropIndex("dbo.DocumentOuts", new[] { "IssuedByUserId" });
            DropIndex("dbo.DocumentOuts", new[] { "XamlDocumentId" });
            DropIndex("dbo.DocumentOuts", new[] { "Id" });
            DropIndex("dbo.DocumentsFilings", new[] { "DepartmentId" });
            DropIndex("dbo.DocumentsFilings", new[] { "BranchId" });
            DropIndex("dbo.DocumentsFilings", new[] { "DeletedByUserId" });
            DropIndex("dbo.DocumentsFilings", new[] { "FolderId" });
            DropIndex("dbo.DocumentsFilings", new[] { "StatusCode" });
            DropIndex("dbo.DocumentsFilings", new[] { "ReceivedByUserId" });
            DropIndex("dbo.DocumentsFilings", new[] { "UpdatedByUserId" });
            DropIndex("dbo.DocumentsFilings", new[] { "OwnerId" });
            DropIndex("dbo.DocumentsFilings", new[] { "CreatedByUserId" });
            DropIndex("dbo.DocumentsFilings", new[] { "ChildObjectTableId" });
            DropIndex("dbo.DocumentsFilings", new[] { "ObjectTableId" });
            DropIndex("dbo.DocumentsFilings", new[] { "DocumentTypeId" });
            DropIndex("dbo.DocumentsFilings", new[] { "DocumentId" });
            DropIndex("dbo.Documents", new[] { "SmallDocumentId" });
            DropIndex("dbo.CommunicationLogs", new[] { "DocumentId" });
            DropIndex("dbo.CommunicationLogs", new[] { "CreatedByUserId" });
            DropIndex("dbo.CommunicationLogs", new[] { "DocumentsFilingId" });
            DropIndex("dbo.CommunicationLogs", new[] { "DocumentOutId" });
            DropIndex("dbo.CommunicationLogs", new[] { "ObjectTableId" });
            DropIndex("dbo.CommunicationLogs", new[] { "CommunicationStatusTypeCode" });
            DropIndex("dbo.CommunicationLogs", new[] { "CommunicationLogTypeCode" });
            DropIndex("dbo.CommunicationLogs", new[] { "Tenant" });
            DropIndex("dbo.CommunicationAttachments", new[] { "DocumentId" });
            DropIndex("dbo.CommunicationAttachments", new[] { "CommunicationLogId" });
            DropIndex("dbo.CardContacts", new[] { "CardId" });
            DropIndex("dbo.CardContacts", new[] { "ContactId" });
            DropIndex("dbo.AWBSpecialHandlingCodes", new[] { "AirlineId" });
            DropIndex("dbo.ARInvoiceTotalVATs", new[] { "ARInvoiceId" });
            DropIndex("dbo.ARInvoiceTotalVATs", new[] { "VatTypeId" });
            DropIndex("dbo.ARPayments", new[] { "CreditCardTypeId" });
            DropIndex("dbo.ARPayments", new[] { "UpdatedByUserId" });
            DropIndex("dbo.ARPayments", new[] { "BillToAddressId" });
            DropIndex("dbo.ARPayments", new[] { "DebitAccountId" });
            DropIndex("dbo.ARPayments", new[] { "ARPaymentMethodCode" });
            DropIndex("dbo.ARPayments", new[] { "PaymentCurrencyId" });
            DropIndex("dbo.ARPayments", new[] { "StatusCode" });
            DropIndex("dbo.ARPayments", new[] { "ARAccountId" });
            DropIndex("dbo.ARPayments", new[] { "BillToId" });
            DropIndex("dbo.ARPayments", new[] { "BranchId" });
            DropIndex("dbo.ARPayments", new[] { "LocalCurrencyId" });
            DropIndex("dbo.ARPayments", new[] { "PrintByUserId" });
            DropIndex("dbo.ARPayments", new[] { "CreatedByUserId" });
            DropIndex("dbo.ARInvoicePayments", new[] { "ForeignCurrencyId" });
            DropIndex("dbo.ARInvoicePayments", new[] { "ARInvoiceId" });
            DropIndex("dbo.ARInvoicePayments", new[] { "ARPaymentId" });
            DropIndex("dbo.ARInvoiceLines", new[] { "MeasurementId" });
            DropIndex("dbo.ARInvoiceLines", new[] { "VatTypeId" });
            DropIndex("dbo.ARInvoiceLines", new[] { "ForiegnCurrencyId" });
            DropIndex("dbo.ARInvoiceLines", new[] { "ChargesTypeId" });
            DropIndex("dbo.ARInvoiceLines", new[] { "ARInvoiceId" });
            DropIndex("dbo.ARInvoices", new[] { "TransferStatusCode" });
            DropIndex("dbo.ARInvoices", new[] { "BranchId" });
            DropIndex("dbo.ARInvoices", new[] { "UpdatedByUserId" });
            DropIndex("dbo.ARInvoices", new[] { "ProfitCurrencyId" });
            DropIndex("dbo.ARInvoices", new[] { "PrepaidCollectId" });
            DropIndex("dbo.ARInvoices", new[] { "CreatedByUserId" });
            DropIndex("dbo.ARInvoices", new[] { "PaymentTermId" });
            DropIndex("dbo.ARInvoices", new[] { "CancelledByARInvoiceId" });
            DropIndex("dbo.ARInvoices", new[] { "StatusCode" });
            DropIndex("dbo.ARInvoices", new[] { "LocalCurrencyId" });
            DropIndex("dbo.ARInvoices", new[] { "InvoiceCurrencyId" });
            DropIndex("dbo.ARInvoices", new[] { "IssuedByUserId" });
            DropIndex("dbo.ARInvoices", new[] { "PrintByUserId" });
            DropIndex("dbo.ARInvoices", new[] { "BillToAddressId" });
            DropIndex("dbo.ARInvoices", new[] { "BillToId" });
            DropIndex("dbo.ARInvoices", new[] { "ARInvoiceTypeCode" });
            DropIndex("dbo.ARInvoiceEntities", new[] { "ObjectTableId" });
            DropIndex("dbo.ARInvoiceEntities", new[] { "ARInvoiceId" });
            DropIndex("dbo.APInvoiceTotalVATs", new[] { "APInvoiceId" });
            DropIndex("dbo.APInvoiceTotalVATs", new[] { "VatTypeId" });
            DropIndex("dbo.APPayments", new[] { "CreditCardTypeId" });
            DropIndex("dbo.APPayments", new[] { "UpdatedByUserId" });
            DropIndex("dbo.APPayments", new[] { "BranchId" });
            DropIndex("dbo.APPayments", new[] { "VendorAddressId" });
            DropIndex("dbo.APPayments", new[] { "PaymentCurrencyId" });
            DropIndex("dbo.APPayments", new[] { "PaymentMethodCode" });
            DropIndex("dbo.APPayments", new[] { "StatusCode" });
            DropIndex("dbo.APPayments", new[] { "VendorId" });
            DropIndex("dbo.APPayments", new[] { "LocalCurrencyId" });
            DropIndex("dbo.APPayments", new[] { "PrintedByUserId" });
            DropIndex("dbo.APPayments", new[] { "CreatedByUserId" });
            DropIndex("dbo.APInvoicePayments", new[] { "ForeignCurrencyId" });
            DropIndex("dbo.APInvoicePayments", new[] { "APPaymentId" });
            DropIndex("dbo.APInvoicePayments", new[] { "APInvoiceId" });
            DropIndex("dbo.IATACodes", new[] { "AirlineId" });
            DropIndex("dbo.IATACodes", new[] { "DueTypeCode" });
            DropIndex("dbo.IATACodes", new[] { "Code" });
            DropIndex("dbo.ChargesTypes", new[] { "PayableAccountId" });
            DropIndex("dbo.ChargesTypes", new[] { "ReceivableAccountId" });
            DropIndex("dbo.ChargesTypes", new[] { "DueTypeCode" });
            DropIndex("dbo.ChargesTypes", new[] { "IATACodeId" });
            DropIndex("dbo.ChargesTypes", new[] { "VatTypeId" });
            DropIndex("dbo.ChargesTypes", new[] { "ChargesGroupCode" });
            DropIndex("dbo.ChargesTypes", new[] { "ContainerMeasurementId" });
            DropIndex("dbo.ChargesTypes", new[] { "MeasurementId" });
            DropIndex("dbo.APInvoiceLines", new[] { "ForiegnCurrencyId" });
            DropIndex("dbo.APInvoiceLines", new[] { "VatTypeId" });
            DropIndex("dbo.APInvoiceLines", new[] { "ChargesTypeId" });
            DropIndex("dbo.APInvoiceLines", new[] { "APInvoiceId" });
            DropIndex("dbo.APInvoices", new[] { "TransferStatusCode" });
            DropIndex("dbo.APInvoices", new[] { "BranchId" });
            DropIndex("dbo.APInvoices", new[] { "UpdatedByUserId" });
            DropIndex("dbo.APInvoices", new[] { "ProfitCurrencyId" });
            DropIndex("dbo.APInvoices", new[] { "CreatedByUserId" });
            DropIndex("dbo.APInvoices", new[] { "StatusCode" });
            DropIndex("dbo.APInvoices", new[] { "LocalCurrencyId" });
            DropIndex("dbo.APInvoices", new[] { "InvoiceCurrencyId" });
            DropIndex("dbo.APInvoices", new[] { "PaymentTermId" });
            DropIndex("dbo.APInvoices", new[] { "VendorId" });
            DropIndex("dbo.APInvoiceEntities", new[] { "APInvoiceId" });
            DropIndex("dbo.APInvoiceEntities", new[] { "ObjectTableId" });
            DropIndex("dbo.Features", new[] { "FeatureTypeCode" });
            DropIndex("dbo.Features", new[] { "NameTextCodeId" });
            DropIndex("dbo.Features", new[] { "ObjectTableId" });
            DropIndex("dbo.Queries", new[] { "FeatureId" });
            DropIndex("dbo.Queries", new[] { "NameTextCodeId" });
            DropIndex("dbo.Queries", new[] { "QueryGroupCode" });
            DropIndex("dbo.Queries", new[] { "OriginalQueryId" });
            DropIndex("dbo.Queries", new[] { "ObjectTableId" });
            DropIndex("dbo.Queries", new[] { "UserId" });
            DropIndex("dbo.Tips", new[] { "ObjectTableId" });
            DropIndex("dbo.Tips", new[] { "ShortTextCode" });
            DropIndex("dbo.Screens", new[] { "ObjectTableId" });
            DropIndex("dbo.ObjectTables", new[] { "NewButtonTextCodeId" });
            DropIndex("dbo.ObjectTables", new[] { "ObjectTableTypeCode" });
            DropIndex("dbo.ObjectTables", new[] { "MainTipCode" });
            DropIndex("dbo.ObjectTables", new[] { "DescriptionTextCodeId" });
            DropIndex("dbo.ObjectTables", new[] { "HeaderScreenId" });
            DropIndex("dbo.TextCodes", new[] { "SpellCheckedByUserId" });
            DropIndex("dbo.TextCodes", new[] { "TextCodeTypeCode" });
            DropIndex("dbo.TextCodes", new[] { "ObjectTableId" });
            DropIndex("dbo.ObjectFields", new[] { "AgentPermissionTypeCode" });
            DropIndex("dbo.ObjectFields", new[] { "CustomerPermissionTypeCode" });
            DropIndex("dbo.ObjectFields", new[] { "MultiTableId" });
            DropIndex("dbo.ObjectFields", new[] { "ShortNameTextCodeId" });
            DropIndex("dbo.ObjectFields", new[] { "ListTextCodeId" });
            DropIndex("dbo.ObjectFields", new[] { "LookUpTableId" });
            DropIndex("dbo.ObjectFields", new[] { "HelpTextCodeId" });
            DropIndex("dbo.ObjectFields", new[] { "DataTypeCode" });
            DropIndex("dbo.ObjectFields", new[] { "ObjectTableId" });
            DropIndex("dbo.ObjectFields", new[] { "FullNameTextCodeId" });
            DropIndex("dbo.AdvancedQueryFilters", new[] { "UserId" });
            DropIndex("dbo.AdvancedQueryFilters", new[] { "ObjectFieldId" });
            DropIndex("dbo.AdvancedQueryFilters", new[] { "QueryId" });
            DropIndex("dbo.Accounts1", new[] { "AccountTypeCode" });
            DropIndex("dbo.States", new[] { "CountryId" });
            DropIndex("dbo.Addresses", new[] { "CardId" });
            DropIndex("dbo.Addresses", new[] { "CountryId" });
            DropIndex("dbo.Addresses", new[] { "AddressTypeId" });
            DropIndex("dbo.Addresses", new[] { "StateId" });
            DropIndex("dbo.Tenants", new[] { "CustomerId" });
            DropIndex("dbo.Tenants", new[] { "VatMandatoryTypeCode" });
            DropIndex("dbo.Tenants", new[] { "VatMandatoryCountryId" });
            DropIndex("dbo.Tenants", new[] { "VatUniqueCountryId" });
            DropIndex("dbo.Tenants", new[] { "VatUniqueTypeCode" });
            DropIndex("dbo.Tenants", new[] { "WeightMeasurementUnitCode" });
            DropIndex("dbo.Tenants", new[] { "AgentId" });
            DropIndex("dbo.Tenants", new[] { "ProfitCurrencyId" });
            DropIndex("dbo.Tenants", new[] { "PaymentTermId" });
            DropIndex("dbo.Tenants", new[] { "PasswordPolicyCode" });
            DropIndex("dbo.Tenants", new[] { "QuoteSaleCurrencyId" });
            DropIndex("dbo.Tenants", new[] { "OtherChargesCurrencyId" });
            DropIndex("dbo.Tenants", new[] { "FreightCurrencyId" });
            DropIndex("dbo.Tenants", new[] { "MasterImportOtherPrepaidCollectId" });
            DropIndex("dbo.Tenants", new[] { "MasterImportFreightPrepaidCollectId" });
            DropIndex("dbo.Tenants", new[] { "MasterExportOtherPrepaidCollectId" });
            DropIndex("dbo.Tenants", new[] { "MasterExportFreightPrepaidCollectId" });
            DropIndex("dbo.Tenants", new[] { "ImportOtherPrepaidCollectId" });
            DropIndex("dbo.Tenants", new[] { "ImportFreightPrepaidCollectId" });
            DropIndex("dbo.Tenants", new[] { "ExportOtherPrepaidCollectId" });
            DropIndex("dbo.Tenants", new[] { "ExportFreightPrepaidCollectId" });
            DropIndex("dbo.Tenants", new[] { "GrossWeightUnitCode" });
            DropIndex("dbo.Tenants", new[] { "VolumeUnitCode" });
            DropIndex("dbo.Tenants", new[] { "DimensionsUnitCode" });
            DropIndex("dbo.Tenants", new[] { "AddressId" });
            DropIndex("dbo.Tenants", new[] { "CurrencyId" });
            DropIndex("dbo.AccountingSettings", new[] { "AccountingSystemCode" });
            DropIndex("dbo.AccountingSettings", new[] { "Id" });
            DropIndex("dbo.Reconciliations", new[] { "CreatedByUserId" });
            DropIndex("dbo.Reconciliations", new[] { "AccountId" });
            DropIndex("dbo.ReconciliationLines", new[] { "TransactionId" });
            DropIndex("dbo.ReconciliationLines", new[] { "CurrencyId" });
            DropIndex("dbo.ReconciliationLines", new[] { "ReconciliationId" });
            DropIndex("dbo.LedgerTransactions", new[] { "OpenAmountCurrencyId" });
            DropIndex("dbo.LedgerTransactions", new[] { "OppositeAccountId" });
            DropIndex("dbo.LedgerTransactions", new[] { "CurrencyId" });
            DropIndex("dbo.LedgerTransactions", new[] { "AccountId" });
            DropIndex("dbo.LedgerTransactions", new[] { "ControlAccountId" });
            DropIndex("dbo.LedgerTransactions", new[] { "JournalId", "JournalLineNumber" });
            DropIndex("dbo.Journals", new[] { "VoidedByUserId" });
            DropIndex("dbo.Journals", new[] { "OriginalJournalId" });
            DropIndex("dbo.Journals", new[] { "ApprovedByUserId" });
            DropIndex("dbo.Journals", new[] { "UpdatedByUserId" });
            DropIndex("dbo.Journals", new[] { "AccountingEntityCode" });
            DropIndex("dbo.Journals", new[] { "CreatedByUserId" });
            DropIndex("dbo.Journals", new[] { "StatusCode" });
            DropIndex("dbo.Journals", new[] { "TypeCode" });
            DropIndex("dbo.JournalLines", new[] { "CurrencyId" });
            DropIndex("dbo.JournalLines", new[] { "CreditAccountId" });
            DropIndex("dbo.JournalLines", new[] { "CreditControlAccountId" });
            DropIndex("dbo.JournalLines", new[] { "DebitAccountId" });
            DropIndex("dbo.JournalLines", new[] { "DebitControlAccountId" });
            DropIndex("dbo.JournalLines", new[] { "ActionCode" });
            DropIndex("dbo.JournalLines", new[] { "JournalId" });
            DropIndex("dbo.GLAccountTotalsByMonth", new[] { "CurrencyId" });
            DropIndex("dbo.GLAccountTotalsByMonth", new[] { "AccountId" });
            DropIndex("dbo.UserLastLogins", new[] { "Id" });
            DropIndex("dbo.Warehouses", new[] { "Id" });
            DropIndex("dbo.Vendors", new[] { "Id" });
            DropIndex("dbo.Truckers", new[] { "Id" });
            DropIndex("dbo.ShippingLines", new[] { "ShippingAgentId" });
            DropIndex("dbo.ShippingLines", new[] { "Id" });
            DropIndex("dbo.ShippingAgents", new[] { "Id" });
            DropIndex("dbo.Countries", new[] { "GlobalZoneId" });
            DropIndex("dbo.CustomAgents", new[] { "Id" });
            DropIndex("dbo.Airlines", new[] { "Id" });
            DropIndex("dbo.Agents", new[] { "Id" });
            DropIndex("dbo.Cards", new[] { "ClassifierId" });
            DropIndex("dbo.Cards", new[] { "CollectorId" });
            DropIndex("dbo.Cards", new[] { "SharedLogisticsInvitationStatusCode" });
            DropIndex("dbo.Cards", new[] { "UpdatedByUserId" });
            DropIndex("dbo.Cards", new[] { "CreatedByUserId" });
            DropIndex("dbo.Cards", new[] { "SalesmanUserId" });
            DropIndex("dbo.Cards", new[] { "CountryId" });
            DropIndex("dbo.Cards", new[] { "PrimaryContactId" });
            DropIndex("dbo.Cards", new[] { "ImageDetailId" });
            DropIndex("dbo.Cards", new[] { "VatTypeId" });
            DropIndex("dbo.Cards", new[] { "InvoiceCurrencyId" });
            DropIndex("dbo.Cards", new[] { "PartnerTypeId" });
            DropIndex("dbo.Cards", new[] { "PaymentTermId" });
            DropIndex("dbo.ContactLastLogins", new[] { "Id" });
            DropIndex("dbo.Contacts", new[] { "IndexColor" });
            DropIndex("dbo.Contacts", new[] { "ContactDoneMethodCode" });
            DropIndex("dbo.Contacts", new[] { "ImageDetailId" });
            DropIndex("dbo.Users", new[] { "FreelancerId" });
            DropIndex("dbo.Users", new[] { "DepartmentId" });
            DropIndex("dbo.Users", new[] { "BranchId" });
            DropIndex("dbo.Users", new[] { "BusinessUnitId" });
            DropIndex("dbo.Users", new[] { "ProductTypeCode" });
            DropIndex("dbo.Users", new[] { "DistributorCode" });
            DropIndex("dbo.Users", new[] { "Id" });
            DropIndex("dbo.Customers", new[] { "MediatorId" });
            DropIndex("dbo.Customers", new[] { "CustomsAgentId" });
            DropIndex("dbo.Customers", new[] { "ForwarderId" });
            DropIndex("dbo.Customers", new[] { "FreelancerId" });
            DropIndex("dbo.Customers", new[] { "RegionId" });
            DropIndex("dbo.Customers", new[] { "BeforeDeactiveStatusCode" });
            DropIndex("dbo.Customers", new[] { "CustomerStatusCode" });
            DropIndex("dbo.Customers", new[] { "LeadSourceId" });
            DropIndex("dbo.Customers", new[] { "IndustryId" });
            DropIndex("dbo.Customers", new[] { "CustomerSizeId" });
            DropIndex("dbo.Customers", new[] { "CollectorId" });
            DropIndex("dbo.Customers", new[] { "ClassifierId" });
            DropIndex("dbo.Customers", new[] { "SalesmanUserId" });
            DropIndex("dbo.Customers", new[] { "AccountManagerUserId" });
            DropIndex("dbo.Customers", new[] { "BillToId" });
            DropIndex("dbo.Customers", new[] { "RankId" });
            DropIndex("dbo.Customers", new[] { "Id" });
            DropIndex("dbo.GLAccounts", new[] { "VendorId" });
            DropIndex("dbo.GLAccounts", new[] { "ClientId" });
            DropIndex("dbo.GLAccounts", new[] { "PreviousChartOfAccountsId" });
            DropIndex("dbo.GLAccounts", new[] { "AutomaticReconcileId" });
            DropIndex("dbo.GLAccounts", new[] { "ControlAccountId" });
            DropIndex("dbo.GLAccounts", new[] { "ReconcileMethodCode" });
            DropIndex("dbo.GLAccounts", new[] { "ChartOfAccountsTypeCode" });
            DropIndex("dbo.GLAccounts", new[] { "ChartOfAccountsId" });
            DropIndex("dbo.GLAccounts", new[] { "RevenueExpenseType" });
            DropIndex("dbo.GLAccounts", new[] { "CurrencyId" });
            DropIndex("dbo.GLAccounts", new[] { "AccountTypeCode" });
            DropIndex("dbo.GLAccountBalancesByYear", new[] { "CurrencyId" });
            DropIndex("dbo.GLAccountBalancesByYear", new[] { "AccountId" });
            DropIndex("dbo.ChartOfAccounts", new[] { "TypeCode" });
            DropIndex("dbo.ChartOfAccounts", new[] { "ParentId" });
            DropIndex("dbo.AutomaticReconcileMethods", new[] { "AutomaticReconcile3" });
            DropIndex("dbo.AutomaticReconcileMethods", new[] { "AutomaticReconcile2" });
            DropIndex("dbo.AutomaticReconcileMethods", new[] { "AutomaticReconcile1" });
            DropIndex("dbo.AccountingPeriods", new[] { "PeriodTypeCode" });
            DropTable("dbo.UserPermittedProducts");
            DropTable("dbo.QuotePackages");
            DropTable("dbo.AdditionalServices");
            DropTable("dbo.CustomerCompetitorProducts");
            DropTable("dbo.CustomerCompetitors");
            DropTable("dbo.Competitors");
            DropTable("dbo.CustomerProductLocationActualDatas");
            DropTable("dbo.CustomerProductLocations");
            DropTable("dbo.CustomerProductActualDatas");
            DropTable("dbo.CustomerProducts");
            DropTable("dbo.ProductPeriods");
            DropTable("dbo.UserPermittedBranches");
            DropTable("dbo.CommunicationLogSteps");
            DropTable("dbo.ContactLoginLogs");
            DropTable("dbo.ReportGroups");
            DropTable("dbo.Reports");
            DropTable("dbo.ChargeTypeAccountings");
            DropTable("dbo.VatTypePercentages");
            DropTable("dbo.ValidationTypes");
            DropTable("dbo.UserLoginLogs");
            DropTable("dbo.Translations");
            DropTable("dbo.TranslationHeaders");
            DropTable("dbo.TraceEvents");
            DropTable("dbo.TipsVisibilities");
            DropTable("dbo.TermsofUseSignatures");
            DropTable("dbo.TermsofUses");
            DropTable("dbo.TenantSettings");
            DropTable("dbo.TarrifSteps");
            DropTable("dbo.TarrifFromToTypes");
            DropTable("dbo.TarrifFromToes");
            DropTable("dbo.TarrifTypes");
            DropTable("dbo.TarrifHeaders");
            DropTable("dbo.TarrifCharges");
            DropTable("dbo.SpecialServices");
            DropTable("dbo.ShipmentReceivables");
            DropTable("dbo.ShipmentReceivableLineStatus");
            DropTable("dbo.ShipmentPickUpDeliveryPackages");
            DropTable("dbo.ShipmentPickUpDeliveries");
            DropTable("dbo.ShipmentPayables");
            DropTable("dbo.ShipmentPayableLineStatus");
            DropTable("dbo.ShipmentPayableAmountTypes");
            DropTable("dbo.ShipmentOrderPackages");
            DropTable("dbo.ShipmentCarrierStatuses");
            DropTable("dbo.ShipmentAWBPrintOnlies");
            DropTable("dbo.SharedLogisticsUpdateStatus");
            DropTable("dbo.SharedLogisticsUpdates");
            DropTable("dbo.ScreenModifications");
            DropTable("dbo.ScreenFields");
            DropTable("dbo.RuleConditionFields");
            DropTable("dbo.FeatureAccessLevels");
            DropTable("dbo.RoleFeatures");
            DropTable("dbo.Restrictions");
            DropTable("dbo.RatesTables");
            DropTable("dbo.QuotePriceSteps");
            DropTable("dbo.QuoteCharges");
            DropTable("dbo.QueryColumns");
            DropTable("dbo.PickUpDeliveryTypes");
            DropTable("dbo.PickUpDeliveryFromToTypes");
            DropTable("dbo.Packages");
            DropTable("dbo.PackageFeatures");
            DropTable("dbo.ObjectTableTabs");
            DropTable("dbo.TriggerTypes");
            DropTable("dbo.RuleTypes");
            DropTable("dbo.RuleNotificationTypes");
            DropTable("dbo.ObjectTableRules");
            DropTable("dbo.ObjectTableRuleFields");
            DropTable("dbo.ObjectTableHelperControls");
            DropTable("dbo.ObjectFieldValidations");
            DropTable("dbo.ObjectFieldModifications");
            DropTable("dbo.MenuTypes");
            DropTable("dbo.MenusTables");
            DropTable("dbo.MenuButtons");
            DropTable("dbo.MenuButtonGroups");
            DropTable("dbo.MAWBStacks");
            DropTable("dbo.MarkUpTypes");
            DropTable("dbo.RateClasses");
            DropTable("dbo.ShipmentCommodities");
            DropTable("dbo.ShipmentPackages");
            DropTable("dbo.InsideShipmentPackages");
            DropTable("dbo.FormCustomFields1");
            DropTable("dbo.SpecialServicesTypes");
            DropTable("dbo.ShipmentReceivableStatus");
            DropTable("dbo.ShipmentPayableStatus");
            DropTable("dbo.ManifestStatus");
            DropTable("dbo.FWBStatus");
            DropTable("dbo.ShipmentMasterDatas");
            DropTable("dbo.ShipmentLevels");
            DropTable("dbo.ShipmentCustomerTypes");
            DropTable("dbo.ShipmentComputedFields");
            DropTable("dbo.Vessels");
            DropTable("dbo.NextLegs");
            DropTable("dbo.MoveTypes");
            DropTable("dbo.HybridPartners");
            DropTable("dbo.AccountingInformationIdentifiers");
            DropTable("dbo.Shipments");
            DropTable("dbo.QuoteStages");
            DropTable("dbo.TransportModes");
            DropTable("dbo.ShipmentTypes");
            DropTable("dbo.QuoteRatings");
            DropTable("dbo.QuoteTypes");
            DropTable("dbo.QuoteTemplateTextDesigns");
            DropTable("dbo.BorderTypes");
            DropTable("dbo.QuoteTemplateTableDesigns");
            DropTable("dbo.QuoteTemplateSettings");
            DropTable("dbo.QuoteTemplates");
            DropTable("dbo.QuoteCustomerTypes");
            DropTable("dbo.QuoteClosingReasons");
            DropTable("dbo.PackageTypes");
            DropTable("dbo.Incoterms");
            DropTable("dbo.Ports");
            DropTable("dbo.Quotes");
            DropTable("dbo.FollowUps");
            DropTable("dbo.FHLStatus");
            DropTable("dbo.EventTypeCategories");
            DropTable("dbo.EventTypes");
            DropTable("dbo.EntityStatus");
            DropTable("dbo.EntityLastUpdates");
            DropTable("dbo.EntityLastActivityTypes");
            DropTable("dbo.EntityLastActivities");
            DropTable("dbo.EntityDates");
            DropTable("dbo.DocumentTypeTemplates");
            DropTable("dbo.DocumentTypeCustomFields1");
            DropTable("dbo.DocumentTypeCopies");
            DropTable("dbo.DocumentOutCopies");
            DropTable("dbo.Directions");
            DropTable("dbo.DescriptionOfGoods");
            DropTable("dbo.DBIdCounters");
            DropTable("dbo.DataBaseProperties");
            DropTable("dbo.CustomTables");
            DropTable("dbo.CustomPickLists");
            DropTable("dbo.CounterStats");
            DropTable("dbo.CounterLastNumbers");
            DropTable("dbo.Counters");
            DropTable("dbo.CounterDefinitions");
            DropTable("dbo.ContactTenants");
            DropTable("dbo.ContactTenantRoleSet");
            DropTable("dbo.DocumentFolders");
            DropTable("dbo.TemplateFormats");
            DropTable("dbo.DocumentTypeCategories");
            DropTable("dbo.DocumentsDataProviders");
            DropTable("dbo.RoleTypes");
            DropTable("dbo.Roles");
            DropTable("dbo.DocumentTypes");
            DropTable("dbo.DocumentStatus");
            DropTable("dbo.DocumentOuts");
            DropTable("dbo.DocumentsFilings");
            DropTable("dbo.SmallDocuments");
            DropTable("dbo.Documents");
            DropTable("dbo.CommunicationStatusTypes");
            DropTable("dbo.CommunicationLogTypes");
            DropTable("dbo.CommunicationLogs");
            DropTable("dbo.CommunicationAttachments");
            DropTable("dbo.CategoryTypes");
            DropTable("dbo.CardContacts");
            DropTable("dbo.AWBStatus");
            DropTable("dbo.AWBSpecialHandlingCodes");
            DropTable("dbo.AWBChargesCodes");
            DropTable("dbo.ARInvoiceTotalVATs");
            DropTable("dbo.ARPaymentStatus");
            DropTable("dbo.ARPaymentMethods");
            DropTable("dbo.ARPayments");
            DropTable("dbo.ARInvoicePayments");
            DropTable("dbo.ARInvoiceLines");
            DropTable("dbo.ARInvoiceTransferStatus");
            DropTable("dbo.ARInvoiceStatus");
            DropTable("dbo.ARInvoiceTypes");
            DropTable("dbo.ARInvoices");
            DropTable("dbo.ARInvoiceEntities");
            DropTable("dbo.APInvoiceTypes");
            DropTable("dbo.APInvoiceTotalVATs");
            DropTable("dbo.APPaymentStatus");
            DropTable("dbo.APPaymentMethods");
            DropTable("dbo.CreditCardTypes");
            DropTable("dbo.APPayments");
            DropTable("dbo.APInvoicePayments");
            DropTable("dbo.Measurements");
            DropTable("dbo.DueTypes");
            DropTable("dbo.IATACodes");
            DropTable("dbo.ChargesGroups");
            DropTable("dbo.ChargesTypes");
            DropTable("dbo.APInvoiceLines");
            DropTable("dbo.APInvoiceTransferStatus");
            DropTable("dbo.APInvoiceStatus");
            DropTable("dbo.APInvoices");
            DropTable("dbo.APInvoiceEntities");
            DropTable("dbo.QueryGroups");
            DropTable("dbo.FeatureTypes");
            DropTable("dbo.Features");
            DropTable("dbo.Queries");
            DropTable("dbo.TextCodeTypes");
            DropTable("dbo.ObjectTableTypes");
            DropTable("dbo.Tips");
            DropTable("dbo.Screens");
            DropTable("dbo.ObjectTables");
            DropTable("dbo.TextCodes");
            DropTable("dbo.FieldDataTypes");
            DropTable("dbo.PermissionTypes");
            DropTable("dbo.ObjectFields");
            DropTable("dbo.AdvancedQueryFilters");
            DropTable("dbo.AccountTypes");
            DropTable("dbo.Accounts1");
            DropTable("dbo.VolumeUnits");
            DropTable("dbo.VatUniqueTypes");
            DropTable("dbo.VatMandatoryTypes");
            DropTable("dbo.PasswordPolicies");
            DropTable("dbo.PrepaidCollects");
            DropTable("dbo.DimensionsUnits");
            DropTable("dbo.WeightUnits");
            DropTable("dbo.States");
            DropTable("dbo.AddressTypes");
            DropTable("dbo.Addresses");
            DropTable("dbo.Tenants");
            DropTable("dbo.AccountingSettings");
            DropTable("dbo.AccountingSystems");
            DropTable("dbo.TestEntities");
            DropTable("dbo.Reconciliations");
            DropTable("dbo.ReconciliationLines");
            DropTable("dbo.LedgerTransactions");
            DropTable("dbo.JournalTypes");
            DropTable("dbo.JournalStatusTypes");
            DropTable("dbo.Journals");
            DropTable("dbo.JournalLines");
            DropTable("dbo.JournalActionTypes");
            DropTable("dbo.GLAccountTotalsByMonth");
            DropTable("dbo.RevenueExpenseTypes");
            DropTable("dbo.ReconcileMethods");
            DropTable("dbo.GLAccountTypes");
            DropTable("dbo.Regions");
            DropTable("dbo.Ranks");
            DropTable("dbo.LeadSources");
            DropTable("dbo.Industries");
            DropTable("dbo.CustomerSizes");
            DropTable("dbo.CustomerStatus");
            DropTable("dbo.UserLastLogins");
            DropTable("dbo.ProductTypes");
            DropTable("dbo.Warehouses");
            DropTable("dbo.Vendors");
            DropTable("dbo.VatTypes");
            DropTable("dbo.Truckers");
            DropTable("dbo.ShippingLines");
            DropTable("dbo.ShippingAgents");
            DropTable("dbo.SharedLogisticsInvitationStatus");
            DropTable("dbo.PaymentTerms");
            DropTable("dbo.PartnerTypes");
            DropTable("dbo.GlobalZones");
            DropTable("dbo.Countries");
            DropTable("dbo.CustomAgents");
            DropTable("dbo.Airlines");
            DropTable("dbo.Agents");
            DropTable("dbo.Cards");
            DropTable("dbo.Distributors");
            DropTable("dbo.Departments");
            DropTable("dbo.ImageDetails");
            DropTable("dbo.ContactLastLogins");
            DropTable("dbo.ContactDoneMethods");
            DropTable("dbo.ColorIndexes");
            DropTable("dbo.Contacts");
            DropTable("dbo.BusinessUnits");
            DropTable("dbo.Branches");
            DropTable("dbo.Users");
            DropTable("dbo.Customers");
            DropTable("dbo.GLAccounts");
            DropTable("dbo.Currencies");
            DropTable("dbo.GLAccountBalancesByYear");
            DropTable("dbo.ChartOfAccountsTypes");
            DropTable("dbo.ChartOfAccounts");
            DropTable("dbo.AutomaticReconciles");
            DropTable("dbo.AutomaticReconcileMethods");
            DropTable("dbo.PeriodTypes");
            DropTable("dbo.AccountingPeriods");
            DropTable("dbo.AccountingEntities");
        }
    }
}
