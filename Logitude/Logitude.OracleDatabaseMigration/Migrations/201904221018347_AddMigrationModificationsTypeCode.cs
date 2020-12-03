namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationModificationsTypeCode : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("Customs.VendorCommissions");
            AddColumn("Customs.VendorCommissions", "ModificationsTypeCode", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AddPrimaryKey("Customs.VendorCommissions", new[] { "VendorId", "CustomerId", "ModificationsTypeCode" });
            CreateIndex("Customs.VendorCommissions", "ModificationsTypeCode");
            AddForeignKey("Customs.VendorCommissions", "ModificationsTypeCode", "Customs.ModificationAndDiscountTypes", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.VendorCommissions", "ModificationsTypeCode", "Customs.ModificationAndDiscountTypes");
            DropIndex("Customs.VendorCommissions", new[] { "ModificationsTypeCode" });
            DropPrimaryKey("Customs.VendorCommissions");
            DropColumn("Customs.VendorCommissions", "ModificationsTypeCode");
            AddPrimaryKey("Customs.VendorCommissions", new[] { "VendorId", "CustomerId" });
        }
    }
}
