namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddSpaceLinesBeforeFieldsToQuoteTemplateSettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeContainers", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforePackages", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeQuoteHeaders", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeQuoteDetails", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeHeaders", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeFooters", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforePerContainers", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeFooters");
            DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeHeaders");
            DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeQuoteDetails");
            DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeQuoteHeaders");
            DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforePackages");
            DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeContainers");
            DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforePerContainers");

        }
    }
}
