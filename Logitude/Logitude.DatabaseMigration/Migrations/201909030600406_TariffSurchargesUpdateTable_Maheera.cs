namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TariffSurchargesUpdateTable_Maheera : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TariffSurchargesUpdates",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        TariffVersionId = c.String(maxLength: 15, unicode: false),
                        ChargeTypeId = c.String(maxLength: 15, unicode: false),
                        MinPrice = c.Decimal(precision: 18, scale: 3),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 3),
                        StartDate = c.DateTime(),
                        LinesUpdated = c.Int(),
                        From = c.String(),
                        To = c.String(),
                        Index = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChargesTypes", t => t.ChargeTypeId)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.ChargeTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TariffSurchargesUpdates", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.TariffSurchargesUpdates", "ChargeTypeId", "dbo.ChargesTypes");
            DropIndex("dbo.TariffSurchargesUpdates", new[] { "ChargeTypeId" });
            DropIndex("dbo.TariffSurchargesUpdates", new[] { "CreatedByUserId" });
            DropTable("dbo.TariffSurchargesUpdates");
        }
    }
}
