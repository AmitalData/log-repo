namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAPPaymentExternalAmount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountingSettings", "EnableAPPaymentExternalPayment", c => c.Boolean(nullable: false));
            AddColumn("dbo.APPayments", "ExternalPaymentAmount", c => c.Double());
            AddColumn("dbo.APPayments", "ExternalPaymentDate", c => c.DateTime());
            AddColumn("dbo.APPayments", "ExternalPaymentNotes", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.APPayments", "ExternalPaymentNotes");
            DropColumn("dbo.APPayments", "ExternalPaymentDate");
            DropColumn("dbo.APPayments", "ExternalPaymentAmount");
            DropColumn("dbo.AccountingSettings", "EnableAPPaymentExternalPayment");
        }
    }
}
