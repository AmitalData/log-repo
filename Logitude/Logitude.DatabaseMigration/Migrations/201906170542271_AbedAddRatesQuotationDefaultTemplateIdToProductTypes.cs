namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddRatesQuotationDefaultTemplateIdToProductTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductTypes", "RoutingRQuoteDefaultTemplateId", c => c.String(maxLength: 15, unicode: false));

            CreateIndex("dbo.ProductTypes", "RoutingRQuoteDefaultTemplateId");
            AddForeignKey("dbo.ProductTypes", "RoutingRQuoteDefaultTemplateId", "dbo.QuoteTemplates", "Id");

            AddColumn("dbo.ProductTypeModifications", "RoutingRQuoteDefaultTemplateId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.ProductTypeModifications", "QuotationDefaultTemplateId", c => c.String(maxLength: 15, unicode: false));

        }
        
        public override void Down()
        {

            DropForeignKey("dbo.ProductTypes", "RoutingRQuoteDefaultTemplateId", "dbo.QuoteTemplates");
            DropIndex("dbo.ProductTypes", new[] { "RoutingRQuoteDefaultTemplateId" });

            DropColumn("dbo.ProductTypes", "RoutingRQuoteDefaultTemplateId");

            AlterColumn("dbo.ProductTypeModifications", "QuotationDefaultTemplateId", c => c.String());
            DropColumn("dbo.ProductTypeModifications", "RoutingRQuoteDefaultTemplateId");
        }
    }
}
