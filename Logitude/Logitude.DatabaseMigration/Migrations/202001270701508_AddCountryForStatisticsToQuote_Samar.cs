namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryForStatisticsToQuote_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quotes", "CountryForStatisticsId", c => c.String(maxLength: 15, unicode: false));            
            CreateIndex("dbo.Quotes", "CountryForStatisticsId");            
            AddForeignKey("dbo.Quotes", "CountryForStatisticsId", "dbo.Countries", "Id");            
        }
        
        public override void Down()
        {            
            DropForeignKey("dbo.Quotes", "CountryForStatisticsId", "dbo.Countries");            
            DropIndex("dbo.Quotes", new[] { "CountryForStatisticsId" });           
            DropColumn("dbo.Quotes", "CountryForStatisticsId");           
        }
    }
}
