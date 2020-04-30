namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeMinmumBillingDataType : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE GLAccounts ALTER COLUMN MinimumInterestInvoiceBilling int null");
        }
        
        public override void Down()
        {
        }
    }
}
