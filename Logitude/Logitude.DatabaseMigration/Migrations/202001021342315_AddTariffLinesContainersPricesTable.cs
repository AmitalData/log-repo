namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTariffLinesContainersPricesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TariffLinesContainersPrices",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        TariffId = c.String(nullable: false, maxLength: 15, unicode: false),
                        TariffLineId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SurchargeId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Price1 = c.Decimal(precision: 18, scale: 3),
                        Price2 = c.Decimal(precision: 18, scale: 3),
                        Price3 = c.Decimal(precision: 18, scale: 3),
                        Price4 = c.Decimal(precision: 18, scale: 3),
                        Price5 = c.Decimal(precision: 18, scale: 3),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChargesTypes", t => t.SurchargeId)
                .ForeignKey("dbo.Tariffs", t => t.TariffId)
                .ForeignKey("dbo.TariffLines", t => t.TariffLineId)
                .Index(t => t.TariffId)
                .Index(t => t.TariffLineId)
                .Index(t => t.SurchargeId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TariffLinesContainersPrices", "TariffLineId", "dbo.TariffLines");
            DropForeignKey("dbo.TariffLinesContainersPrices", "TariffId", "dbo.Tariffs");
            DropForeignKey("dbo.TariffLinesContainersPrices", "SurchargeId", "dbo.ChargesTypes");
            DropIndex("dbo.TariffLinesContainersPrices", new[] { "SurchargeId" });
            DropIndex("dbo.TariffLinesContainersPrices", new[] { "TariffLineId" });
            DropIndex("dbo.TariffLinesContainersPrices", new[] { "TariffId" });
            DropTable("dbo.TariffLinesContainersPrices");
        }
    }
}
