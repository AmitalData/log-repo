namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableFields_JournalMoreData : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JournalMoreDatas", "TaxReportId", "dbo.TaxReports");
            DropForeignKey("dbo.JournalMoreDatas", "TaxReportStatusCode", "dbo.TaxReportStatuses");
            DropIndex("dbo.JournalMoreDatas", new[] { "TaxReportId" });
            DropIndex("dbo.JournalMoreDatas", new[] { "TaxReportStatusCode" });
            AddColumn("dbo.JournalMoreDatas", "IsLedgerCreated", c => c.Boolean(nullable: false));
            DropColumn("dbo.JournalMoreDatas", "TaxReportId");
            DropColumn("dbo.JournalMoreDatas", "TaxReportStatusCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JournalMoreDatas", "TaxReportStatusCode", c => c.String(maxLength: 1, unicode: false));
            AddColumn("dbo.JournalMoreDatas", "TaxReportId", c => c.String(maxLength: 15, unicode: false));
            DropColumn("dbo.JournalMoreDatas", "IsLedgerCreated");
            CreateIndex("dbo.JournalMoreDatas", "TaxReportStatusCode");
            CreateIndex("dbo.JournalMoreDatas", "TaxReportId");
            AddForeignKey("dbo.JournalMoreDatas", "TaxReportStatusCode", "dbo.TaxReportStatuses", "Code");
            AddForeignKey("dbo.JournalMoreDatas", "TaxReportId", "dbo.TaxReports", "Id");
        }
    }
}
