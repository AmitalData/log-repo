namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APPaymentAutomaticPaymentChequeFieldMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.APPayments", "AutomaticPaymentCheque", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.APPayments", "AutomaticPaymentCheque");
        }
    }
}
