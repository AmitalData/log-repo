namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTemperatureFieldType : DbMigration
    {
        public override void Up()
        {            
            AlterColumn("dbo.ShipmentPackages", "Temperature", c => c.String(maxLength: 8, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ShipmentPackages", "Temperature", c => c.Double());
        }
    }
}
