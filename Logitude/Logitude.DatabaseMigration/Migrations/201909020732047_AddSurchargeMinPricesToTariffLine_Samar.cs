namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSurchargeMinPricesToTariffLine_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TariffLines", "Surcharge1MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge2MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge3MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge4MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge5MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge6MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge7MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge8MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge9MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge10MinPrice", c => c.Decimal(precision: 18, scale: 3));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TariffLines", "Surcharge10MinPrice");
            DropColumn("dbo.TariffLines", "Surcharge9MinPrice");
            DropColumn("dbo.TariffLines", "Surcharge8MinPrice");
            DropColumn("dbo.TariffLines", "Surcharge7MinPrice");
            DropColumn("dbo.TariffLines", "Surcharge6MinPrice");
            DropColumn("dbo.TariffLines", "Surcharge5MinPrice");
            DropColumn("dbo.TariffLines", "Surcharge4MinPrice");
            DropColumn("dbo.TariffLines", "Surcharge3MinPrice");
            DropColumn("dbo.TariffLines", "Surcharge2MinPrice");
            DropColumn("dbo.TariffLines", "Surcharge1MinPrice");
        }
    }
}
