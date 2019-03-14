namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TEXTbla : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.CourierMasters", "StorageSiteCode", c => c.String(maxLength: 20, unicode: false));
            CreateIndex("Customs.CourierMasters", "StorageSiteCode");
            AddForeignKey("Customs.CourierMasters", "StorageSiteCode", "Customs.DeliverySiteTypes", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.CourierMasters", "StorageSiteCode", "Customs.DeliverySiteTypes");
            DropIndex("Customs.CourierMasters", new[] { "StorageSiteCode" });
            DropColumn("Customs.CourierMasters", "StorageSiteCode");
        }
    }
}
