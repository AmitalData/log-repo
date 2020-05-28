namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEstimatedProfitInQuotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quotes", "EstimatedProfitInLocal", c => c.Double());
            AddColumn("dbo.Quotes", "EstimatedProfitInProfit", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Quotes", "EstimatedProfitInProfit");
            DropColumn("dbo.Quotes", "EstimatedProfitInLocal");
        }
    }
}
