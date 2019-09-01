namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCopyQuoteRatesSetting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuoteSettings", "CopyExchangeRates", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuoteSettings", "CopyExchangeRates");
        }
    }
}
