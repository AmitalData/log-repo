namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUpdateMethodCodeToTariffSurchargesTable_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TariffSurchargesUpdates", "UpdateMethodCode", c => c.String(maxLength: 3, unicode: false));
            CreateIndex("dbo.TariffSurchargesUpdates", "UpdateMethodCode");
            AddForeignKey("dbo.TariffSurchargesUpdates", "UpdateMethodCode", "dbo.TariffSurchargesUpdateMethods", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TariffSurchargesUpdates", "UpdateMethodCode", "dbo.TariffSurchargesUpdateMethods");
            DropIndex("dbo.TariffSurchargesUpdates", new[] { "UpdateMethodCode" });
            DropColumn("dbo.TariffSurchargesUpdates", "UpdateMethodCode");
        }
    }
}
