namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JournalAdditionalDataMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JournalAdditionalDatas",
                c => new
                    {
                        JournalId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        TaxReportId = c.String(maxLength: 15, unicode: false),
                        TaxReportStatusCode = c.String(maxLength: 1, unicode: false),
                    })
                .PrimaryKey(t => t.JournalId)
                .ForeignKey("dbo.Journals", t => t.JournalId)
                .ForeignKey("dbo.TaxReports", t => t.TaxReportId)
                .ForeignKey("dbo.TaxReportStatuses", t => t.TaxReportStatusCode)
                .Index(t => t.JournalId)
                .Index(t => t.TaxReportId)
                .Index(t => t.TaxReportStatusCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JournalAdditionalDatas", "TaxReportStatusCode", "dbo.TaxReportStatuses");
            DropForeignKey("dbo.JournalAdditionalDatas", "TaxReportId", "dbo.TaxReports");
            DropForeignKey("dbo.JournalAdditionalDatas", "JournalId", "dbo.Journals");
            DropIndex("dbo.JournalAdditionalDatas", new[] { "TaxReportStatusCode" });
            DropIndex("dbo.JournalAdditionalDatas", new[] { "TaxReportId" });
            DropIndex("dbo.JournalAdditionalDatas", new[] { "JournalId" });
            DropTable("dbo.JournalAdditionalDatas");
        }
    }
}
