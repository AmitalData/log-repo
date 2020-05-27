namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePickupDeliveryNumberLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ShipmentPickUpDeliveries", "PickUpDeliveryNumber", c => c.String(nullable: false, maxLength: 25, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ShipmentPickUpDeliveries", "PickUpDeliveryNumber", c => c.String(nullable: false, maxLength: 20, unicode: false));
        }
    }
}
