namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddNewFieldShowSaleMinMaxToQuoteTemplateSettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuoteTemplateSettings", "ShowSaleMaxMinAmountPackages", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowSaleMaxMinAmountContainers", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuoteTemplateSettings", "ShowSaleMaxMinAmountContainers");
            DropColumn("dbo.QuoteTemplateSettings", "ShowSaleMaxMinAmountPackages");
        }
    }
}
