namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfitColumnsToQuote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quotes", "ProfitCurrencyId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Quotes", "ProfitExchangeRate", c => c.Double());
            CreateIndex("dbo.Quotes", "ProfitCurrencyId");
            AddForeignKey("dbo.Quotes", "ProfitCurrencyId", "dbo.Currencies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quotes", "ProfitCurrencyId", "dbo.Currencies");
            DropIndex("dbo.Quotes", new[] { "ProfitCurrencyId" });
            DropColumn("dbo.Quotes", "ProfitExchangeRate");
            DropColumn("dbo.Quotes", "ProfitCurrencyId");;
        }
    }
}
