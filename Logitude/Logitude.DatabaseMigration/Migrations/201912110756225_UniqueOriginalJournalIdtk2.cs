namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueOriginalJournalIdtk2 : DbMigration
    {
        public override void Up()
        {
            //IF THERE IS A PROBLEM TALK TO MOHAMMAD
            Sql("CREATE UNIQUE NONCLUSTERED INDEX [UC_Journal_ORG] ON[dbo].[Journals]([OriginalJournalId] ASC) WHERE([OriginalJournalId] IS NOT NULL)");
            //CreateTable(
            //    "dbo.ExternalPageAdditionalDatas",
            //    c => new
            //        {
            //            ObjectTableId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            EntityId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            LastPageNumber = c.String(maxLength: 15, unicode: false),
            //            LastPageEndDate = c.DateTime(),
            //            LastPageCloseBalance = c.Decimal(precision: 16, scale: 2),
            //        })
            //    .PrimaryKey(t => new { t.ObjectTableId, t.EntityId });

            //CreateTable(
            //    "dbo.InterestBasesPeriods",
            //    c => new
            //        {
            //            InterestBaseTypeId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            LineNumber = c.Int(nullable: false),
            //            Tenant = c.Int(nullable: false),
            //            CreateDate = c.DateTime(nullable: false),
            //            CreatedByUserId = c.String(maxLength: 15, unicode: false),
            //            UpdateDate = c.DateTime(nullable: false),
            //            UpdatedByUserId = c.String(maxLength: 15, unicode: false),
            //            InterestBaseStartDate = c.DateTime(nullable: false),
            //            InterestRate = c.Decimal(nullable: false, precision: 4, scale: 2),
            //        })
            //    .PrimaryKey(t => new { t.InterestBaseTypeId, t.LineNumber })
            //    .ForeignKey("dbo.Users", t => t.CreatedByUserId)
            //    .ForeignKey("dbo.InterestBasesTypes", t => t.InterestBaseTypeId)
            //    .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
            //    .Index(t => t.InterestBaseTypeId)
            //    .Index(t => t.CreatedByUserId)
            //    .Index(t => t.UpdatedByUserId);

            //CreateTable(
            //    "dbo.InterestBasesTypes",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            CreateDate = c.DateTime(nullable: false),
            //            CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            UpdateDate = c.DateTime(nullable: false),
            //            UpdatedByUserId = c.String(maxLength: 15, unicode: false),
            //            SearchFields = c.String(),
            //            Code = c.String(nullable: false, maxLength: 4, unicode: false),
            //            LocalName = c.String(maxLength: 256),
            //            EnglishName = c.String(maxLength: 256, unicode: false),
            //            Description = c.String(maxLength: 1024),
            //            InActive = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Users", t => t.CreatedByUserId)
            //    .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
            //    .Index(t => t.CreatedByUserId)
            //    .Index(t => t.UpdatedByUserId);

            //AddColumn("dbo.Tenants", "HideFCLAllIn", c => c.Boolean(nullable: false));
            //AddColumn("dbo.VatTypes", "PayablesExternalId", c => c.String(maxLength: 25, unicode: false));
            //AddColumn("dbo.VatTypes", "ReceivablesExternalId", c => c.String(maxLength: 25, unicode: false));
            //AddColumn("dbo.AdvancedQueryFilters", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            //AddColumn("dbo.ObjectFields", "FieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            //AddColumn("dbo.APPayments", "AccountingCancelationDate", c => c.DateTime());
            //AddColumn("dbo.APPayments", "DontIncludeInDeductionReport", c => c.Boolean(nullable: false));
            //AddColumn("dbo.APPayments", "CancelationNotes", c => c.String());
            //AddColumn("dbo.ARInvoices", "DateForInterest", c => c.DateTime());
            //AddColumn("dbo.GLAccounts", "AllowEditChequePayToName", c => c.Boolean(nullable: false));
            //AddColumn("dbo.BIReports", "LastRunDate", c => c.DateTime(nullable: false));
            //AddColumn("dbo.BIReports", "LastRunByUserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //AddColumn("dbo.QueryColumns", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            //AddColumn("dbo.ScreenFields", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
            //CreateIndex("dbo.ObjectFields", "FieldCode", unique: true);
            //CreateIndex("dbo.BIReports", "LastRunByUserId");
            //AddForeignKey("dbo.BIReports", "LastRunByUserId", "dbo.Users", "Id");
            //DropColumn("dbo.ARInvoices", "DateForVATInterest");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ARInvoices", "DateForVATInterest", c => c.DateTime());
            DropForeignKey("dbo.InterestBasesPeriods", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.InterestBasesPeriods", "InterestBaseTypeId", "dbo.InterestBasesTypes");
            DropForeignKey("dbo.InterestBasesTypes", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.InterestBasesTypes", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.InterestBasesPeriods", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.BIReports", "LastRunByUserId", "dbo.Users");
            DropIndex("dbo.InterestBasesTypes", new[] { "UpdatedByUserId" });
            DropIndex("dbo.InterestBasesTypes", new[] { "CreatedByUserId" });
            DropIndex("dbo.InterestBasesPeriods", new[] { "UpdatedByUserId" });
            DropIndex("dbo.InterestBasesPeriods", new[] { "CreatedByUserId" });
            DropIndex("dbo.InterestBasesPeriods", new[] { "InterestBaseTypeId" });
            DropIndex("dbo.BIReports", new[] { "LastRunByUserId" });
            DropIndex("dbo.ObjectFields", new[] { "FieldCode" });
            DropColumn("dbo.ScreenFields", "ObjectFieldCode");
            DropColumn("dbo.QueryColumns", "ObjectFieldCode");
            DropColumn("dbo.BIReports", "LastRunByUserId");
            DropColumn("dbo.BIReports", "LastRunDate");
            DropColumn("dbo.GLAccounts", "AllowEditChequePayToName");
            DropColumn("dbo.ARInvoices", "DateForInterest");
            DropColumn("dbo.APPayments", "CancelationNotes");
            DropColumn("dbo.APPayments", "DontIncludeInDeductionReport");
            DropColumn("dbo.APPayments", "AccountingCancelationDate");
            DropColumn("dbo.ObjectFields", "FieldCode");
            DropColumn("dbo.AdvancedQueryFilters", "ObjectFieldCode");
            DropColumn("dbo.VatTypes", "ReceivablesExternalId");
            DropColumn("dbo.VatTypes", "PayablesExternalId");
            DropColumn("dbo.Tenants", "HideFCLAllIn");
            DropTable("dbo.InterestBasesTypes");
            DropTable("dbo.InterestBasesPeriods");
            DropTable("dbo.ExternalPageAdditionalDatas");
        }
    }
}
