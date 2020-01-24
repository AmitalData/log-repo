namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addShipmentComputedFieldColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentComputedFields", "OperationallyClosedByUserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShipmentComputedFields", "OperationallyClosedByUserName");
        }
    }
}
