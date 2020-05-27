namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edit_Fields_In_GLAccount : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GLAccounts", "ActiveForInterest", c => c.Boolean());
            AlterColumn("dbo.GLAccounts", "ActiveForInterestCreditInvoice", c => c.Boolean());
            AlterColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling", c => c.Decimal(precision: 5, scale: 0));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling", c => c.Decimal(nullable: false, precision: 5, scale: 0));
            AlterColumn("dbo.GLAccounts", "ActiveForInterestCreditInvoice", c => c.Boolean(nullable: false));
            AlterColumn("dbo.GLAccounts", "ActiveForInterest", c => c.Boolean(nullable: false));
        }
    }
}
