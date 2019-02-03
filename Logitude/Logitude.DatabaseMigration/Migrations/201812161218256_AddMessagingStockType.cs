namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessagingStockType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MessagingStocks", "StockType", c => c.String(maxLength: 10, unicode: false));

            Sql("update MessagingStocks set StockType = 'Champ'");
        }
        
        public override void Down()
        {
            DropColumn("dbo.MessagingStocks", "StockType");
        }
    }
}
