namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JournalMoreDataFieldsMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Journals", "TaxReportStatusCode", "dbo.TaxReportStatuses");
            DropIndex("dbo.Journals", new[] { "TaxReportStatusCode" });
            AddColumn("dbo.JournalMoreDatas", "TaxReportId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.JournalMoreDatas", "TaxReportStatusCode", c => c.String(maxLength: 1, unicode: false));
            CreateIndex("dbo.JournalMoreDatas", "TaxReportId");
            CreateIndex("dbo.JournalMoreDatas", "TaxReportStatusCode");
            AddForeignKey("dbo.JournalMoreDatas", "TaxReportId", "dbo.TaxReports", "Id");
            AddForeignKey("dbo.JournalMoreDatas", "TaxReportStatusCode", "dbo.TaxReportStatuses", "Code");
            DropColumn("dbo.Journals", "TaxReportId");
            DropColumn("dbo.Journals", "TaxReportStatusCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Journals", "TaxReportStatusCode", c => c.String(maxLength: 1, unicode: false));
            AddColumn("dbo.Journals", "TaxReportId", c => c.String(maxLength: 15, unicode: false));
            DropForeignKey("dbo.JournalMoreDatas", "TaxReportStatusCode", "dbo.TaxReportStatuses");
            DropForeignKey("dbo.JournalMoreDatas", "TaxReportId", "dbo.TaxReports");
            DropIndex("dbo.JournalMoreDatas", new[] { "TaxReportStatusCode" });
            DropIndex("dbo.JournalMoreDatas", new[] { "TaxReportId" });
            DropColumn("dbo.JournalMoreDatas", "TaxReportStatusCode");
            DropColumn("dbo.JournalMoreDatas", "TaxReportId");
            CreateIndex("dbo.Journals", "TaxReportStatusCode");
            AddForeignKey("dbo.Journals", "TaxReportStatusCode", "dbo.TaxReportStatuses", "Code");
        }
    }
}
