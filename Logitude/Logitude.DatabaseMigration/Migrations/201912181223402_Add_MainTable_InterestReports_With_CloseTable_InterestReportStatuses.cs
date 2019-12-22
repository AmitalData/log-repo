namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_MainTable_InterestReports_With_CloseTable_InterestReportStatuses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InterestReports",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(),
                        CreatedByUserId = c.String(maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(),
                        UpdatedByUserId = c.String(maxLength: 15, unicode: false),
                        GLAccountId = c.String(maxLength: 15, unicode: false),
                        ReportNumber = c.String(maxLength: 15, unicode: false),
                        InterestCalculationDate = c.DateTime(),
                        TotalAmount = c.Decimal(precision: 18, scale: 2),
                        OpenBalance = c.Decimal(precision: 18, scale: 2),
                        CloseBalance = c.Decimal(precision: 18, scale: 2),
                        ARinvoiceId = c.String(maxLength: 15, unicode: false),
                        InvoiceAmount = c.Decimal(precision: 18, scale: 2),
                        GLAccountInterestCreditLimit = c.Decimal(precision: 18, scale: 2),
                        InterestReportStatusCode = c.String(maxLength: 4, unicode: false),
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
                        LocalName = c.String(maxLength: 30, unicode: false),
                        SearchFields = c.String(),
                        EnglishName = c.String(nullable: false, maxLength: 60, unicode: false),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterestReports", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.InterestReports", "InterestReportStatusCode", "dbo.InterestReportStatuses");
            DropForeignKey("dbo.InterestReports", "GLAccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.InterestReports", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.InterestReports", "ARinvoiceId", "dbo.ARInvoices");
            DropIndex("dbo.InterestReports", new[] { "InterestReportStatusCode" });
            DropIndex("dbo.InterestReports", new[] { "ARinvoiceId" });
            DropIndex("dbo.InterestReports", new[] { "GLAccountId" });
            DropIndex("dbo.InterestReports", new[] { "UpdatedByUserId" });
            DropIndex("dbo.InterestReports", new[] { "CreatedByUserId" });
            DropTable("dbo.InterestReportStatuses");
            DropTable("dbo.InterestReports");
        }
    }
}
