namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPickUpDeliveryPackageHarmonize_Samar : DbMigration
    {
        public override void Up()
        {
            
            CreateTable(
                "dbo.PickUpDeliveryPackageHarmonizes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        PackageId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Harmonize = c.String(nullable: false, maxLength: 60, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ShipmentPickUpDeliveryPackages", t => t.PackageId)
                .Index(t => t.PackageId);
            
            AddColumn("dbo.ShipmentPickUpDeliveryPackages", "IsMultiHarmonize", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PickUpDeliveryPackageHarmonizes", "PackageId", "dbo.ShipmentPickUpDeliveryPackages");
            DropIndex("dbo.PickUpDeliveryPackageHarmonizes", new[] { "PackageId" });
            DropColumn("dbo.ShipmentPickUpDeliveryPackages", "IsMultiHarmonize");
            DropTable("dbo.PickUpDeliveryPackageHarmonizes");
        }
    }
}
