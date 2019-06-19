namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAdddNewFieldsShowHeaderLabelsPackagesToQuoteTemplateSettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuoteTemplateSettings", "ShowHeaderLabelsPackages", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowHeaderLabelsContainers", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuoteTemplateSettings", "ShowHeaderLabelsContainers");
            DropColumn("dbo.QuoteTemplateSettings", "ShowHeaderLabelsPackages");
        }
    }
}
