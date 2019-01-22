namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationWeightValueCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.CourierMasters", "WeightValueCode", c => c.String(maxLength: 3, unicode: false));
            CreateIndex("Customs.CourierMasters", "WeightValueCode");
            AddForeignKey("Customs.CourierMasters", "WeightValueCode", "Customs.FreightPaymentMethods", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.CourierMasters", "WeightValueCode", "Customs.FreightPaymentMethods");
            DropIndex("Customs.CourierMasters", new[] { "WeightValueCode" });
            DropColumn("Customs.CourierMasters", "WeightValueCode");
        }
    }
}
