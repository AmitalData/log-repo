namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Fields_For_GLAccounts_Table : DbMigration
    {
        public override void Up()
        {

            AddColumn("dbo.GLAccounts", "ActiveForInterest", c => c.Boolean(nullable: false));
            AddColumn("dbo.GLAccounts", "InterestCalculationStartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.GLAccounts", "ActiveForInterestCreditInvoice", c => c.Boolean(nullable: false));
            AddColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling", c => c.Decimal(nullable: false, precision: 5, scale: 0));
 
        }
        
        public override void Down()
        {
            DropColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling");
            DropColumn("dbo.GLAccounts", "ActiveForInterestCreditInvoice");
            DropColumn("dbo.GLAccounts", "InterestCalculationStartDate");
            DropColumn("dbo.GLAccounts", "ActiveForInterest");
        }
    }
}
