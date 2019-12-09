namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddToFromTypeCodeFieldToWarehouseEntry : DbMigration
    {
        public override void Up()
        {

            AddColumn("dbo.WarehouseEntries", "ToTypeCode", c => c.String(maxLength: 4, unicode: false));
            AddColumn("dbo.WarehouseEntries", "FromTypeCode", c => c.String(maxLength: 4, unicode: false));
            AddColumn("dbo.WarehouseEntries", "FromCountryId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseEntries", "ToCountryId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.WarehouseEntries", "FromCountryId");
            CreateIndex("dbo.WarehouseEntries", "ToCountryId");
            AddForeignKey("dbo.WarehouseEntries", "FromCountryId", "dbo.Countries", "Id");
            AddForeignKey("dbo.WarehouseEntries", "ToCountryId", "dbo.Countries", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WarehouseEntries", "ToCountryId", "dbo.Countries");
            DropForeignKey("dbo.WarehouseEntries", "FromCountryId", "dbo.Countries");
            DropIndex("dbo.WarehouseEntries", new[] { "ToCountryId" });
            DropIndex("dbo.WarehouseEntries", new[] { "FromCountryId" });
            DropColumn("dbo.WarehouseEntries", "ToCountryId");
            DropColumn("dbo.WarehouseEntries", "FromCountryId");
            DropColumn("dbo.WarehouseEntries", "FromTypeCode");
            DropColumn("dbo.WarehouseEntries", "ToTypeCode");
        }
    }
}
