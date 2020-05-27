namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTariffSurchargesUpdateMethodTable_Maheera : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TariffSurchargesUpdates", "ChargeTypeId", "dbo.ChargesTypes");
            DropIndex("dbo.TariffSurchargesUpdates", new[] { "ChargeTypeId" });
            CreateTable(
                "dbo.TariffSurchargesUpdateMethods",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 3, unicode: false),
                        Name = c.String(maxLength: 40, unicode: false),
                        SearchFields = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            AddColumn("dbo.TariffSurchargesUpdates", "TariffId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TariffSurchargesUpdates", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.TariffSurchargesUpdates", "Surcharges", c => c.String());
            DropColumn("dbo.TariffSurchargesUpdates", "TariffVersionId");
            DropColumn("dbo.TariffSurchargesUpdates", "ChargeTypeId");
            DropColumn("dbo.TariffSurchargesUpdates", "MinPrice");
            DropColumn("dbo.TariffSurchargesUpdates", "Price");
            DropColumn("dbo.TariffSurchargesUpdates", "Index");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TariffSurchargesUpdates", "Index", c => c.Int());
            AddColumn("dbo.TariffSurchargesUpdates", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            AddColumn("dbo.TariffSurchargesUpdates", "MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffSurchargesUpdates", "ChargeTypeId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TariffSurchargesUpdates", "TariffVersionId", c => c.String(maxLength: 15, unicode: false));
            DropColumn("dbo.TariffSurchargesUpdates", "Surcharges");
            DropColumn("dbo.TariffSurchargesUpdates", "Version");
            DropColumn("dbo.TariffSurchargesUpdates", "TariffId");
            DropTable("dbo.TariffSurchargesUpdateMethods");
            CreateIndex("dbo.TariffSurchargesUpdates", "ChargeTypeId");
            AddForeignKey("dbo.TariffSurchargesUpdates", "ChargeTypeId", "dbo.ChargesTypes", "Id");
        }
    }
}
