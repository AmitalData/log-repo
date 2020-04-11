namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransitTimeFieldToTariff_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TariffLines", "TransitTime", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TariffLines", "TransitTime");
        }
    }
}
