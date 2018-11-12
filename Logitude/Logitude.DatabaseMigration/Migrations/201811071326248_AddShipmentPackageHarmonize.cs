namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShipmentPackageHarmonize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShipmentPackageHarmonize",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        PackageId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Harmonize = c.String(nullable: false, maxLength: 60, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ShipmentPackages", t => t.PackageId)
                .Index(t => t.PackageId);
            
            AddColumn("dbo.ShipmentPackages", "IsMultiHarmonize", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShipmentPackageHarmonize", "PackageId", "dbo.ShipmentPackages");
            DropIndex("dbo.ShipmentPackageHarmonize", new[] { "PackageId" });
            DropColumn("dbo.ShipmentPackages", "IsMultiHarmonize");
            DropTable("dbo.ShipmentPackageHarmonize");
        }
    }
}
