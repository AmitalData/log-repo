namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewField_NumberOfAgingMonths : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.AccountingSettings", "AllowManualARPaymentNumber", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Quotes", "CountryForStatisticsId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.ARInvoices", "BillToGLAccountId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.ARPayments", "IsPaymentNumberManuallySet", c => c.Boolean(nullable: false));
            //AddColumn("dbo.GLAccounts", "NameForPrintingCheques", c => c.String(maxLength: 1000));
            AddColumn("dbo.FullAccountingSettings", "NumberOfAgingMonths", c => c.Int());
            //CreateIndex("dbo.Quotes", "CountryForStatisticsId");
            //AddForeignKey("dbo.Quotes", "CountryForStatisticsId", "dbo.Countries", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Quotes", "CountryForStatisticsId", "dbo.Countries");
            //DropIndex("dbo.Quotes", new[] { "CountryForStatisticsId" });
            DropColumn("dbo.FullAccountingSettings", "NumberOfAgingMonths");
            //DropColumn("dbo.GLAccounts", "NameForPrintingCheques");
            //DropColumn("dbo.ARPayments", "IsPaymentNumberManuallySet");
            //DropColumn("dbo.ARInvoices", "BillToGLAccountId");
            //DropColumn("dbo.Quotes", "CountryForStatisticsId");
            //DropColumn("dbo.AccountingSettings", "AllowManualARPaymentNumber");
        }
    }
}
