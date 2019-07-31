namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Shipment_Facts_Fields_Khalid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "Commodity", c => c.String(maxLength: 15));
            AddColumn("dbo.Shipments", "FirstPickupLocation", c => c.String(maxLength: 100));
            AddColumn("dbo.Shipments", "ContainersNumbers", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "ContainersNumbers");
            DropColumn("dbo.Shipments", "FirstPickupLocation");
            DropColumn("dbo.Shipments", "Commodity");
        }
    }
}
