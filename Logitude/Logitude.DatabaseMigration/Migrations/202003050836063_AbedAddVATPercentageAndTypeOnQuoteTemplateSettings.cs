namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddVATPercentageAndTypeOnQuoteTemplateSettings : DbMigration
    {
        public override void Up()
        {

            AddColumn("dbo.QuoteTemplateSettings", "ShowVATTypePackages", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowVATTypeContainers", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowVATPercentagePackages", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowVATPercentageContainers", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuoteTemplateSettings", "ShowVATPercentageContainers");
            DropColumn("dbo.QuoteTemplateSettings", "ShowVATPercentagePackages");
            DropColumn("dbo.QuoteTemplateSettings", "ShowVATTypeContainers");
            DropColumn("dbo.QuoteTemplateSettings", "ShowVATTypePackages");
        }
    }
}
