namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDefaultSortByToDWObjectTable_Rabaia : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.JournalAdditionalDatas", "TaxReportStatusCode", "dbo.TaxReportStatuses");
            //DropIndex("dbo.JournalAdditionalDatas", new[] { "TaxReportStatusCode" });
            //AddColumn("dbo.Tenants", "LocalAddressId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.BatchTaskExecutions", "CallStack", c => c.String());
            AddColumn("dbo.DWObjectTables", "DefaultFilterBy", c => c.String(maxLength: 50, unicode: false));
            //AddColumn("dbo.GLAccountMoreDatas", "TotFutureOpenChequesInLocalCur", c => c.Decimal(precision: 16, scale: 2));
            //AddColumn("dbo.JournalAdditionalDatas", "TaxReportTransmitStatusCode", c => c.String(maxLength: 3, unicode: false));
            //CreateIndex("dbo.Tenants", "LocalAddressId");
            //CreateIndex("dbo.JournalAdditionalDatas", "TaxReportTransmitStatusCode");
            //AddForeignKey("dbo.Tenants", "LocalAddressId", "dbo.Addresses", "Id");
            //AddForeignKey("dbo.JournalAdditionalDatas", "TaxReportTransmitStatusCode", "dbo.TaxReportLineTransmitStatuses", "Code");
            //DropColumn("dbo.JournalAdditionalDatas", "TaxReportStatusCode");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.JournalAdditionalDatas", "TaxReportStatusCode", c => c.String(maxLength: 1, unicode: false));
            //DropForeignKey("dbo.JournalAdditionalDatas", "TaxReportTransmitStatusCode", "dbo.TaxReportLineTransmitStatuses");
            //DropForeignKey("dbo.Tenants", "LocalAddressId", "dbo.Addresses");
            //DropIndex("dbo.JournalAdditionalDatas", new[] { "TaxReportTransmitStatusCode" });
            //DropIndex("dbo.Tenants", new[] { "LocalAddressId" });
            //DropColumn("dbo.JournalAdditionalDatas", "TaxReportTransmitStatusCode");
            //DropColumn("dbo.GLAccountMoreDatas", "TotFutureOpenChequesInLocalCur");
            DropColumn("dbo.DWObjectTables", "DefaultFilterBy");
            //DropColumn("dbo.BatchTaskExecutions", "CallStack");
            //DropColumn("dbo.Tenants", "LocalAddressId");
            //CreateIndex("dbo.JournalAdditionalDatas", "TaxReportStatusCode");
            //AddForeignKey("dbo.JournalAdditionalDatas", "TaxReportStatusCode", "dbo.TaxReportStatuses", "Code");
        }
    }
}
