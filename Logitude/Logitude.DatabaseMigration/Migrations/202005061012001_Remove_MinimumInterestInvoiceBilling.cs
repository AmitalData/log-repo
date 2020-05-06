namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_MinimumInterestInvoiceBilling : DbMigration
    {
        public override void Up()
        {
             DropColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling", c => c.Int());
        }
    }
}
