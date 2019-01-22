namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVehiclePassportName : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.Vehicles", "PassportName", c => c.String(maxLength: 55));
        }
        
        public override void Down()
        {
            DropColumn("Customs.Vehicles", "PassportName");
        }
    }
}
