namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_MinimumInterestInvoiceBilling : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling", c => c.Int(nullable:true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling");
        }
    }
}
