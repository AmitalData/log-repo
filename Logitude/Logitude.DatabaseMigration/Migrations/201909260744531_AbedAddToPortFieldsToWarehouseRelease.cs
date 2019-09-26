namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AbedAddToPortFieldsToWarehouseRelease : DbMigration
    {
        public override void Up()
        {

            //AddColumn("dbo.WarehouseReleases", "ToTypeCode", c => c.String(maxLength: 4, unicode: false));
            //AddColumn("dbo.WarehouseReleases", "ToPartnerCardId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.WarehouseReleases", "ToAddressId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.WarehouseReleases", "ToAddressZipCode", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.WarehouseReleases", "ToAddressCity", c => c.String(maxLength: 25));
            //AddColumn("dbo.WarehouseReleases", "ToAddressCountryId", c => c.String(maxLength: 15, unicode: false));
            //CreateIndex("dbo.WarehouseReleases", "ToPartnerCardId");
            //CreateIndex("dbo.WarehouseReleases", "ToAddressId");
            //AddForeignKey("dbo.WarehouseReleases", "ToAddressId", "dbo.Addresses", "Id");
            //AddForeignKey("dbo.WarehouseReleases", "ToPartnerCardId", "dbo.Cards", "Id");
        }

        public override void Down()
        {
            //DropForeignKey("dbo.WarehouseReleases", "ToPartnerCardId", "dbo.Cards");
            //DropForeignKey("dbo.WarehouseReleases", "ToAddressId", "dbo.Addresses");
            //DropIndex("dbo.WarehouseReleases", new[] { "ToAddressId" });
            //DropIndex("dbo.WarehouseReleases", new[] { "ToPartnerCardId" });
            //DropColumn("dbo.WarehouseReleases", "ToAddressCountryId");
            //DropColumn("dbo.WarehouseReleases", "ToAddressCity");
            //DropColumn("dbo.WarehouseReleases", "ToAddressZipCode");
            //DropColumn("dbo.WarehouseReleases", "ToAddressId");
            //DropColumn("dbo.WarehouseReleases", "ToPartnerCardId");
            //DropColumn("dbo.WarehouseReleases", "ToTypeCode");

        }
    }
}
