namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FullAccountingSettingsAllowMultipleRatesMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FullAccountingSettings", "AllowMultiRatesInInvoiceLines", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FullAccountingSettings", "AllowMultiRatesInInvoiceLines");
        }
    }
}
