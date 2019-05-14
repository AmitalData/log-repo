namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsARPaymentChronologicalDatesToAccountingSettingsTable_Maheera : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AccountingSettings", name: "IsChronologicalDates", newName: "IsARInvoiceChronologicalDates");
            AddColumn("dbo.AccountingSettings", "IsARPaymentChronologicalDates", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccountingSettings", "IsChronologicalDates", c => c.Boolean(nullable: false));
            DropColumn("dbo.AccountingSettings", "IsARPaymentChronologicalDates");
        }
    }
}
