namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddICAOFieldToCustomsAirLines : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.CustomsAirlines", "ICAO", c => c.String(maxLength: 3, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("Customs.CustomsAirlines", "ICAO");
        }
    }
}
