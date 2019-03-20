namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationTruckerIdCourier : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.CourierMasters", "TruckerId", c => c.String(maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("Customs.CourierMasters", "TruckerId");
        }
    }
}
