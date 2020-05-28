namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddQuoteHTMLDocumentIdToQuote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quotes", "QuoteHTMLDocumentId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.Quotes", "QuoteHTMLDocumentId");
            AddForeignKey("dbo.Quotes", "QuoteHTMLDocumentId", "dbo.Documents", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quotes", "QuoteHTMLDocumentId", "dbo.Documents");
            DropIndex("dbo.Quotes", new[] { "QuoteHTMLDocumentId" });
            DropColumn("dbo.Quotes", "QuoteHTMLDocumentId");
        }
    }
}
