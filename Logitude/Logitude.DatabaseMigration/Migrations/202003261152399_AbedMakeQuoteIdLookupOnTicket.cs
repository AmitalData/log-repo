namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedMakeQuoteIdLookupOnTicket : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Tickets", "QuoteId");
            AddForeignKey("dbo.Tickets", "QuoteId", "dbo.Quotes", "Id");
        }
        
        public override void Down()
        {
            
            DropForeignKey("dbo.Tickets", "QuoteId", "dbo.Quotes");
            DropIndex("dbo.Tickets", new[] { "QuoteId" });
    
        }
    }
}
