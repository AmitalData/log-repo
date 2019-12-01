namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APPaymentNewFieldMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.APPayments", "AccountingCancelationDate", c => c.DateTime());
            AddColumn("dbo.APPayments", "DontIncludeInDeductionReport", c => c.Boolean(nullable: false));
            AddColumn("dbo.APPayments", "CancelationNotes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.APPayments", "CancelationNotes");
            DropColumn("dbo.APPayments", "DontIncludeInDeductionReport");
            DropColumn("dbo.APPayments", "AccountingCancelationDate");
        }
    }
}
