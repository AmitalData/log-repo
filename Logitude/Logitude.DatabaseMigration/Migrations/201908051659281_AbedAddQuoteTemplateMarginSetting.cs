namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddQuoteTemplateMarginSetting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuoteTemplateSettings", "QuoteTemplatePDFMarginTop", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "QuoteTemplatePDFMarginBottom", c => c.Int(nullable: false));

        }
        
        public override void Down()
        {

            DropColumn("dbo.QuoteTemplateSettings", "QuoteTemplatePDFMarginBottom");
            DropColumn("dbo.QuoteTemplateSettings", "QuoteTemplatePDFMarginTop");
        }
    }
}
