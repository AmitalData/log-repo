namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_DatesPickupsAndDeliveryComputedfields_Khalid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentComputedFields", "FirstPickupATD", c => c.DateTime());
            AddColumn("dbo.ShipmentComputedFields", "FirstPickupATA", c => c.DateTime());
            AddColumn("dbo.ShipmentComputedFields", "FinalDeliveryETD", c => c.DateTime());
            AddColumn("dbo.ShipmentComputedFields", "FinalDeliveryETA", c => c.DateTime());
            AddColumn("dbo.ShipmentComputedFields", "FinalDeliveryATD", c => c.DateTime());
            AddColumn("dbo.ShipmentComputedFields", "FinalDeliveryATA", c => c.DateTime());         
        }
        
        public override void Down()
        {           
            DropColumn("dbo.ShipmentComputedFields", "FinalDeliveryATA");
            DropColumn("dbo.ShipmentComputedFields", "FinalDeliveryATD");
            DropColumn("dbo.ShipmentComputedFields", "FinalDeliveryETA");
            DropColumn("dbo.ShipmentComputedFields", "FinalDeliveryETD");
            DropColumn("dbo.ShipmentComputedFields", "FirstPickupATA");
            DropColumn("dbo.ShipmentComputedFields", "FirstPickupATD");
        }
    }
}
