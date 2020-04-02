namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTariffCarrierTranslation_Samar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TariffCarrierTranslations",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        PartnerCode = c.String(nullable: false, maxLength: 50, unicode: false),
                        PortId = c.String(nullable: false, maxLength: 15, unicode: false),
                        CarrierId = c.String(nullable: false, maxLength: 15, unicode: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SearchFields = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cards", t => t.CarrierId)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Ports", t => t.PortId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.PortId)
                .Index(t => t.CarrierId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TariffCarrierTranslations", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.TariffCarrierTranslations", "PortId", "dbo.Ports");
            DropForeignKey("dbo.TariffCarrierTranslations", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.TariffCarrierTranslations", "CarrierId", "dbo.Cards");
            DropIndex("dbo.TariffCarrierTranslations", new[] { "UpdatedByUserId" });
            DropIndex("dbo.TariffCarrierTranslations", new[] { "CreatedByUserId" });
            DropIndex("dbo.TariffCarrierTranslations", new[] { "CarrierId" });
            DropIndex("dbo.TariffCarrierTranslations", new[] { "PortId" });
            DropTable("dbo.TariffCarrierTranslations");
        }
    }
}
