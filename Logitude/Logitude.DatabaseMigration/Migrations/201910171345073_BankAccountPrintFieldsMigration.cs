namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BankAccountPrintFieldsMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BankAccounts", "PrintingBranchNumber", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.BankAccounts", "PrintingAccountNumber", c => c.String(maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BankAccounts", "PrintingAccountNumber");
            DropColumn("dbo.BankAccounts", "PrintingBranchNumber");
        }
    }
}
