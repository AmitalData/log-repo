namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewQuoteSettingsFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuoteSettings", "IsSaleAsCostCurrency", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuoteSettings", "IsSaleAsCostCurrency");
        }
    }
}
