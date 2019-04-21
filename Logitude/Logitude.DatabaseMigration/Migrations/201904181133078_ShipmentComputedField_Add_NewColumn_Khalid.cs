namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShipmentComputedField_Add_NewColumn_Khalid : DbMigration
    {
        public override void Up()
        {             
            AddColumn("dbo.Shipments", "ComputedShipmentNumber", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "ComputedShipmentNumber");
        }
    }
}
