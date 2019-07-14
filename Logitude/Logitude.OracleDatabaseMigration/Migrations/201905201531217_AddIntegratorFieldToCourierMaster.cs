namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIntegratorFieldToCourierMaster : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.CourierMasters", "IntegratorCode", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("Customs.CourierMasters", "IntegratorCode");
            AddForeignKey("Customs.CourierMasters", "IntegratorCode", "dbo.Cards", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.CourierMasters", "IntegratorCode", "dbo.Cards");
            DropIndex("Customs.CourierMasters", new[] { "IntegratorCode" });
            DropColumn("Customs.CourierMasters", "IntegratorCode");
        }
    }
}
