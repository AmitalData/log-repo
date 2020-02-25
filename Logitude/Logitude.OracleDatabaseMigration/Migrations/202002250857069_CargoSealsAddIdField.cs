namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CargoSealsAddIdField : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("Customs.CargoSeals");
            AddColumn("Customs.CargoSeals", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AddPrimaryKey("Customs.CargoSeals", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("Customs.CargoSeals");
            DropColumn("Customs.CargoSeals", "Id");
            AddPrimaryKey("Customs.CargoSeals", new[] { "CargoSealIdentifierId", "SealNumber" });
        }
    }
}
