namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTariffFieldsToQuoteCharge_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuoteCharges", "TariffId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.QuoteCharges", "TariffNumber", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.QuoteCharges", "TariffVersion", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuoteCharges", "TariffVersion");
            DropColumn("dbo.QuoteCharges", "TariffNumber");
            DropColumn("dbo.QuoteCharges", "TariffId");
        }
    }
}
