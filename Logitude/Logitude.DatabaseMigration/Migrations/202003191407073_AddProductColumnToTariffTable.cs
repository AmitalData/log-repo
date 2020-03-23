namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductColumnToTariffTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tariffs", "TariffProductId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.Tariffs", "TariffProductId");
            AddForeignKey("dbo.Tariffs", "TariffProductId", "dbo.TariffProducts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tariffs", "TariffProductId", "dbo.TariffProducts");
            DropIndex("dbo.Tariffs", new[] { "TariffProductId" });
            DropColumn("dbo.Tariffs", "TariffProductId");
        }
    }
}
