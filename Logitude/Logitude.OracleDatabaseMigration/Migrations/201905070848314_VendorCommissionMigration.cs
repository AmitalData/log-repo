namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VendorCommissionMigration : DbMigration
    {
        public override void Up()
        {
            //DropPrimaryKey("Customs.VendorCommissions");
         
            //AddColumn("VendorCommissions", "ModificationsTypeCode", c => c.String(nullable: false, maxLength: 3, unicode: false, defaultValue: "I10"));
            //AddPrimaryKey("Customs.VendorCommissions", new[] { "VendorId", "CustomerId", "ModificationsTypeCode" });
            //CreateIndex("Customs.CourierMasters", "StorageSiteCode");
            //CreateIndex("VendorCommissions", "ModificationsTypeCode");
            //AddForeignKey("Customs.CourierMasters", "StorageSiteCode", "Customs.DeliverySiteTypes", "Code");
            //AddForeignKey("Customs.VendorCommissions", "ModificationsTypeCode", "ModificationAndDiscountTypes", "Code");
        }
        
        public override void Down()
        {
        }
    }
}
