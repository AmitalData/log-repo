namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStorageSiteStatusCourier : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.DeclarationCourierStatuses", "StorageSiteStatusCode", c => c.String(maxLength: 3, unicode: false));
            AddColumn("Customs.DeclarationCourierStatuses", "StorageSiteErrorText", c => c.String(maxLength: 1200));
            CreateIndex("Customs.DeclarationCourierStatuses", "StorageSiteStatusCode");
            AddForeignKey("Customs.DeclarationCourierStatuses", "StorageSiteStatusCode", "Customs.MamanStatuses", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.DeclarationCourierStatuses", "StorageSiteStatusCode", "Customs.MamanStatuses");
            DropIndex("Customs.DeclarationCourierStatuses", new[] { "StorageSiteStatusCode" });
            DropColumn("Customs.DeclarationCourierStatuses", "StorageSiteErrorText");
            DropColumn("Customs.DeclarationCourierStatuses", "StorageSiteStatusCode");
        }
    }
}
