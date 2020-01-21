namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFeatureUniqeCodeToReports_rabaia : DbMigration
    {
        public override void Up()
        {
            
            AddColumn("dbo.Reports", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            Sql("update Reports set FeatureUniqeCode = (select FeatureUniqeCode from Features where Id = FeatureId)");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InterestReports", "UpdateDate", c => c.DateTime());
            DropForeignKey("dbo.InterestReportLinesByDates", "InterestReportId", "dbo.InterestReports");
            DropForeignKey("dbo.InterestReportLines", "InterestReportId", "dbo.InterestReports");
            DropForeignKey("dbo.Tickets", "SupportMailboxId", "dbo.SupportMailboxes");
            DropIndex("dbo.InterestReportLinesByDates", new[] { "InterestReportId" });
            DropIndex("dbo.InterestReports", new[] { "GLAccountId" });
            DropIndex("dbo.InterestReportLines", new[] { "InterestTransactionId" });
            DropIndex("dbo.InterestReportLines", new[] { "InterestReportId" });
            DropIndex("dbo.Tickets", new[] { "SupportMailboxId" });
            DropPrimaryKey("dbo.InterestReportLines");
            AlterColumn("dbo.InterestReportStatuses", "LocalName", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.InterestReports", "InterestCalculationDate", c => c.DateTime());
            AlterColumn("dbo.InterestReports", "GLAccountId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.InterestReportLinesByDates", "CalculatedCreditInterestAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.InterestReportLinesByDates", "CalculatedExcepInterestAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.InterestReportLinesByDates", "CalculatedStandInterestAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.InterestReportLinesByDates", "CreditInterestAmount", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.InterestReportLinesByDates", "ExceptionalInterestAmount", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.InterestReportLinesByDates", "StandardInterestAmount", c => c.Decimal(nullable: false, precision: 19, scale: 3));
            AlterColumn("dbo.InterestReportLinesByDates", "LineNumber", c => c.Decimal(nullable: false, precision: 6, scale: 0));
            AlterColumn("dbo.InterestReportLines", "InterestTransactionId", c => c.String(maxLength: 15, unicode: false));
            DropColumn("dbo.Translations", "TextCodeCode");
            DropColumn("dbo.RuleConditionFields", "ObjectFieldCode");
            DropColumn("dbo.Reports", "FeatureUniqeCode");
            DropColumn("dbo.ObjectTableRuleFields", "ObjectFieldCode");
            DropColumn("dbo.ObjectFieldValidations", "ObjectFieldCode");
            DropColumn("dbo.MenuButtons", "LabelTextCodeCode");
            DropColumn("dbo.InterestReports", "UpdateDateTime");
            DropColumn("dbo.Features", "NameTextCodeCode");
            DropColumn("dbo.Tickets", "SupportMailboxId");
            AddPrimaryKey("dbo.InterestReportLines", "InterestReportId");
            CreateIndex("dbo.InterestReports", "GLAccountId");
            CreateIndex("dbo.InterestReportLines", "InterestTransactionId");
        }
    }
}
