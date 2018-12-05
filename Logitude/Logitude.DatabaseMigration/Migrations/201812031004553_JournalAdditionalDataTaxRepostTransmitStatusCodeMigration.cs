namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JournalAdditionalDataTaxRepostTransmitStatusCodeMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JournalAdditionalDatas", "TaxReportStatusCode", "dbo.TaxReportStatuses");
            DropIndex("dbo.JournalAdditionalDatas", new[] { "TaxReportStatusCode" });
            AddColumn("dbo.JournalAdditionalDatas", "TaxReportTransmitStatusCode", c => c.String(maxLength: 3, unicode: false));
            CreateIndex("dbo.JournalAdditionalDatas", "TaxReportTransmitStatusCode");
            AddForeignKey("dbo.JournalAdditionalDatas", "TaxReportTransmitStatusCode", "dbo.TaxReportLineTransmitStatuses", "Code");
            DropColumn("dbo.JournalAdditionalDatas", "TaxReportStatusCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JournalAdditionalDatas", "TaxReportStatusCode", c => c.String(maxLength: 1, unicode: false));
            DropForeignKey("dbo.JournalAdditionalDatas", "TaxReportTransmitStatusCode", "dbo.TaxReportLineTransmitStatuses");
            DropIndex("dbo.JournalAdditionalDatas", new[] { "TaxReportTransmitStatusCode" });
            DropColumn("dbo.JournalAdditionalDatas", "TaxReportTransmitStatusCode");
            CreateIndex("dbo.JournalAdditionalDatas", "TaxReportStatusCode");
            AddForeignKey("dbo.JournalAdditionalDatas", "TaxReportStatusCode", "dbo.TaxReportStatuses", "Code");
        }
    }
}
