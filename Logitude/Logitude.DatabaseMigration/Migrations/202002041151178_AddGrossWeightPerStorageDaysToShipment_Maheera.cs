namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGrossWeightPerStorageDaysToShipment_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "GrossWeightPerStorageDays", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "GrossWeightPerStorageDays");
        }
    }
}
