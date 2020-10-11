namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VendorCommissionMigration : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("Customs.VendorCommissions");
            //AddColumn("Customs.CourierMasters", "StorageSiteCode", c => c.String(maxLength: 20, unicode: false));
            //AddColumn("Customs.CourierMasters", "TruckerId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.DWObjectFields", "HideTree", c => c.Boolean(nullable: false));
            AddColumn("Customs.VendorCommissions", "ModificationsTypeCode", c => c.String(nullable: false, maxLength: 3, unicode: false,defaultValue: "I10"));
            AddPrimaryKey("Customs.VendorCommissions", new[] { "VendorId", "CustomerId", "ModificationsTypeCode" });
            //CreateIndex("Customs.CourierMasters", "StorageSiteCode");
            CreateIndex("Customs.VendorCommissions", "ModificationsTypeCode");
            //AddForeignKey("Customs.CourierMasters", "StorageSiteCode", "Customs.DeliverySiteTypes", "Code");
            AddForeignKey("Customs.VendorCommissions", "ModificationsTypeCode", "Customs.ModificationAndDiscountTypes", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.VendorCommissions", "ModificationsTypeCode", "Customs.ModificationAndDiscountTypes");
            DropForeignKey("Customs.CourierMasters", "StorageSiteCode", "Customs.DeliverySiteTypes");
            DropIndex("Customs.VendorCommissions", new[] { "ModificationsTypeCode" });
            DropIndex("Customs.CourierMasters", new[] { "StorageSiteCode" });
            DropPrimaryKey("Customs.VendorCommissions");
            DropColumn("Customs.VendorCommissions", "ModificationsTypeCode");
            DropColumn("dbo.DWObjectFields", "HideTree");
            DropColumn("Customs.CourierMasters", "TruckerId");
            DropColumn("Customs.CourierMasters", "StorageSiteCode");
            AddPrimaryKey("Customs.VendorCommissions", new[] { "VendorId", "CustomerId" });
        }
    }
}
