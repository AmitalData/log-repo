namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToTenantAndShippingLine_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tenants", "CBSA", c => c.String(maxLength: 5, unicode: false));
            AddColumn("dbo.Tenants", "CAAT", c => c.String(maxLength: 4, unicode: false));
            AddColumn("dbo.ShippingLines", "CBSA", c => c.String(maxLength: 5, unicode: false));
            AddColumn("dbo.ShippingLines", "CAAT", c => c.String(maxLength: 4, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tenants", "CAAT");
            DropColumn("dbo.Tenants", "CBSA");
        }
    }
}
