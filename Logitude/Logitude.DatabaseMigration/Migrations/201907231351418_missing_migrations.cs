namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missing_migrations : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeContainers", c => c.Int(nullable: false));
            //AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforePackages", c => c.Int(nullable: false));
            //AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeQuoteHeaders", c => c.Int(nullable: false));
            //AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeQuoteDetails", c => c.Int(nullable: false));
            //AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeHeaders", c => c.Int(nullable: false));
            //AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeFooters", c => c.Int(nullable: false));
            //AddColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforePerContainers", c => c.Int(nullable: false));

            AddColumn("dbo.ARPayments", "FechaPago", c => c.DateTime());

            //AddColumn("dbo.ShipmentComputedFields", "Commodity", c => c.String());
            //AddColumn("dbo.ShipmentComputedFields", "FirstPickupLocation", c => c.String());
            //AddColumn("dbo.ShipmentComputedFields", "ContainersNumbers", c => c.String());
            //AddColumn("dbo.ShipmentComputedFields", "FirstPickupATD", c => c.DateTime());
            //AddColumn("dbo.ShipmentComputedFields", "FirstPickupATA", c => c.DateTime());
            //AddColumn("dbo.ShipmentComputedFields", "FinalDeliveryETD", c => c.DateTime());
            //AddColumn("dbo.ShipmentComputedFields", "FinalDeliveryETA", c => c.DateTime());
            //AddColumn("dbo.ShipmentComputedFields", "FinalDeliveryATD", c => c.DateTime());
            //AddColumn("dbo.ShipmentComputedFields", "FinalDeliveryATA", c => c.DateTime());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.ShipmentComputedFields", "FinalDeliveryATA");
            //DropColumn("dbo.ShipmentComputedFields", "FinalDeliveryATD");
            //DropColumn("dbo.ShipmentComputedFields", "FinalDeliveryETA");
            //DropColumn("dbo.ShipmentComputedFields", "FinalDeliveryETD");
            //DropColumn("dbo.ShipmentComputedFields", "FirstPickupATA");
            //DropColumn("dbo.ShipmentComputedFields", "FirstPickupATD");
            //DropColumn("dbo.ShipmentComputedFields", "ContainersNumbers");
            //DropColumn("dbo.ShipmentComputedFields", "FirstPickupLocation");
            //DropColumn("dbo.ShipmentComputedFields", "Commodity");
            DropColumn("dbo.ARPayments", "FechaPago");
            //DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforePerContainers");
            //DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeFooters");
            //DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeHeaders");
            //DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeQuoteDetails");
            //DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeQuoteHeaders");
            //DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforePackages");
            //DropColumn("dbo.QuoteTemplateSettings", "SpaceLinesBeforeContainers");
        }
    }
}
