namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingcustoms2905_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.CourierMasters", "StorageSiteCode", c => c.String(maxLength: 20, unicode: false));
            AddColumn("Customs.CourierMasters", "TruckerId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.DWObjectFields", "HideTree", c => c.Boolean(nullable: false));
            CreateIndex("Customs.CourierMasters", "StorageSiteCode");
            AddForeignKey("Customs.CourierMasters", "StorageSiteCode", "Customs.DeliverySiteTypes", "Code");
        }

        public override void Down()
        {
            DropForeignKey("Customs.CourierMasters", "StorageSiteCode", "Customs.DeliverySiteTypes");
            DropIndex("Customs.CourierMasters", new[] { "StorageSiteCode" });
            DropColumn("dbo.DWObjectFields", "HideTree");
            DropColumn("Customs.CourierMasters", "TruckerId");
            DropColumn("Customs.CourierMasters", "StorageSiteCode");
        }
    }
}
