namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APPAymentPaymentChequeIdMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.APPayments", "PaymentChequeId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.APPayments", "PaymentChequeId");
        }
    }
}
