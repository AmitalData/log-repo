namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddQuotationShowIncludedChargesFieldsToQuoteTemplateSettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesPerContainers", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesPackages", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesContainers", c => c.Boolean(nullable: false));

        }
        
        public override void Down()
        {

            DropColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesContainers");
            DropColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesPackages");
            DropColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesPerContainers");
        }
    }
}
