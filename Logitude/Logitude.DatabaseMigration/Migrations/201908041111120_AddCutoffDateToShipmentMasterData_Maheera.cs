namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCutoffDateToShipmentMasterData_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentMasterDatas", "CutoffDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShipmentMasterDatas", "CutoffDate");
        }
    }
}
