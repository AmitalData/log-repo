namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentMethodLocalNameMigration : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.AccountingPaymentMethods", "LocalName", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.AccountingPaymentMethods", "LocalName");
        }
    }
}
