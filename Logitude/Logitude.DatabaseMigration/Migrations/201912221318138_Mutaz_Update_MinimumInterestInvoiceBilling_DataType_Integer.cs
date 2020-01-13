namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Update_MinimumInterestInvoiceBilling_DataType_Integer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InterestReports", "UpdateDateTime", c => c.DateTime());
            DropIndex("dbo.InterestReportLines", new[] { "InterestTransactionId" });
            DropPrimaryKey("dbo.InterestReportLines");
            AlterColumn("dbo.InterestReportLines", "InterestTransactionId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            Sql("IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__GLAccount__Minim__2B76DBC6]') AND type = 'D') BEGIN ALTER TABLE[dbo].[GLAccounts] DROP CONSTRAINT[DF__GLAccount__Minim__2B76DBC6] END");
            AlterColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling", c => c.Int());
            DropColumn("dbo.InterestReports", "UpdateDate");
            AddPrimaryKey("dbo.InterestReportLines", new[] { "InterestReportId", "InterestTransactionId" });
            CreateIndex("dbo.InterestReportLinesByDates", "InterestReportId");
            CreateIndex("dbo.InterestReportLines", "InterestTransactionId");
            CreateIndex("dbo.InterestReportLines", "InterestReportId");
            AddForeignKey("dbo.InterestReportLinesByDates", "InterestReportId", "dbo.InterestReports", "Id");
            AddForeignKey("dbo.InterestReportLines", "InterestReportId", "dbo.InterestReports", "Id");

          
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterestReportLines", "InterestReportId", "dbo.InterestReports");
            DropForeignKey("dbo.InterestReportLinesByDates", "InterestReportId", "dbo.InterestReports");
            DropIndex("dbo.InterestReportLines", new[] { "InterestReportId" });
            DropIndex("dbo.InterestReportLines", new[] { "InterestTransactionId" });
            DropIndex("dbo.InterestReportLinesByDates", new[] { "InterestReportId" });
            DropPrimaryKey("dbo.InterestReportLines");
            AddColumn("dbo.InterestReports", "UpdateDate", c => c.DateTime());
            AlterColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling", c => c.Int());
            AlterColumn("dbo.InterestReportLines", "InterestTransactionId", c => c.String(maxLength: 15, unicode: false));
            AddPrimaryKey("dbo.InterestReportLines", "InterestReportId");
            CreateIndex("dbo.InterestReportLines", "InterestTransactionId");
            DropColumn("dbo.InterestReports", "UpdateDateTime");
        }
    }
}
