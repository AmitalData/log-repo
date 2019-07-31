namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingEcommerceSupportEmailToTenantTable_Rabaia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tenants", "EcommerceSupportEmail", c => c.String(maxLength: 50, unicode: false));
            //AddColumn("dbo.Quotes", "GrossWeightEdited", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Quotes", "ChargeableWeightEdited", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Shipments", "OrderGrossWeightEdited", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Shipments", "OrderChargeableWeightEdited", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Shipments", "OrderChargeableWeightEdited");
            //DropColumn("dbo.Shipments", "OrderGrossWeightEdited");
            //DropColumn("dbo.Quotes", "ChargeableWeightEdited");
            //DropColumn("dbo.Quotes", "GrossWeightEdited");
            DropColumn("dbo.Tenants", "EcommerceSupportEmail");
        }
    }
}
