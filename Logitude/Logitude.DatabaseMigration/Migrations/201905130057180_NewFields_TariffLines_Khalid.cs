namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewFields_TariffLines_Khalid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TariffLines", "Surcharge1Price", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge2Price", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge3Price", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge4Price", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge5Price", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge6Price", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge7Price", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge8Price", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge9Price", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge10Price", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge1PriceText", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.TariffLines", "Surcharge2PriceText", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.TariffLines", "Surcharge3PriceText", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.TariffLines", "Surcharge4PriceText", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.TariffLines", "Surcharge5PriceText", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.TariffLines", "Surcharge6PriceText", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.TariffLines", "Surcharge7PriceText", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.TariffLines", "Surcharge8PriceText", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.TariffLines", "Surcharge9PriceText", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.TariffLines", "Surcharge10PriceText", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TariffLines", "Surcharge10PriceText");
            DropColumn("dbo.TariffLines", "Surcharge9PriceText");
            DropColumn("dbo.TariffLines", "Surcharge8PriceText");
            DropColumn("dbo.TariffLines", "Surcharge7PriceText");
            DropColumn("dbo.TariffLines", "Surcharge6PriceText");
            DropColumn("dbo.TariffLines", "Surcharge5PriceText");
            DropColumn("dbo.TariffLines", "Surcharge4PriceText");
            DropColumn("dbo.TariffLines", "Surcharge3PriceText");
            DropColumn("dbo.TariffLines", "Surcharge2PriceText");
            DropColumn("dbo.TariffLines", "Surcharge1PriceText");
            DropColumn("dbo.TariffLines", "Surcharge10Price");
            DropColumn("dbo.TariffLines", "Surcharge9Price");
            DropColumn("dbo.TariffLines", "Surcharge8Price");
            DropColumn("dbo.TariffLines", "Surcharge7Price");
            DropColumn("dbo.TariffLines", "Surcharge6Price");
            DropColumn("dbo.TariffLines", "Surcharge5Price");
            DropColumn("dbo.TariffLines", "Surcharge4Price");
            DropColumn("dbo.TariffLines", "Surcharge3Price");
            DropColumn("dbo.TariffLines", "Surcharge2Price");
            DropColumn("dbo.TariffLines", "Surcharge1Price");
        }
    }
}
