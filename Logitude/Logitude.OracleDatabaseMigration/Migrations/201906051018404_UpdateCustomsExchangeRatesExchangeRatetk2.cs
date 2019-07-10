namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCustomsExchangeRatesExchangeRatetk2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Customs.CustomsExchangeRates", "ExchangeRate", c => c.Decimal(precision: 15, scale: 10));
        }
        
        public override void Down()
        {
            AlterColumn("Customs.CustomsExchangeRates", "ExchangeRate", c => c.Decimal(precision: 12, scale: 10));
        }
    }
}
