namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_MinimumInterestInvoiceBilling_DataType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling", c => c.Int(nullable: true));
           
        }
        
        public override void Down()
        {
             
        }
    }
}
