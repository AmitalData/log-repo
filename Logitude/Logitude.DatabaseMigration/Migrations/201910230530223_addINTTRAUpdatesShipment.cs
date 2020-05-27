namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addINTTRAUpdatesShipment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShippingLines", "INTTRAUpdatesShipment", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShippingLines", "INTTRAUpdatesShipment");
        }
    }
}
