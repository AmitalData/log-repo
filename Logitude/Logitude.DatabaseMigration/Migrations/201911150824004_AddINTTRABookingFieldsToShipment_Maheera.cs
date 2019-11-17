namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddINTTRABookingFieldsToShipment_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "INTTRABookingError", c => c.String(maxLength: 256, unicode: false));
            AddColumn("dbo.Shipments", "INTTRALastBookingResponse", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "INTTRALastBookingResponse");
            DropColumn("dbo.Shipments", "INTTRABookingError");
        }
    }
}
