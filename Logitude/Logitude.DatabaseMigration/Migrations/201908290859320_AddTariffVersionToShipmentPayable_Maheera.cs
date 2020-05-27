namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTariffVersionToShipmentPayable_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentPayables", "TariffVersion", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShipmentPayables", "TariffVersion");
        }
    }
}
