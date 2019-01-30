namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shipment_number_20 : DbMigration
    {
        public override void Up()
        {
			Sql(@"ALTER TABLE [dbo].[Quotes] drop  CONSTRAINT [UQ_Tenant_QuoteNumber_Quotes]");
			Sql(@"ALTER TABLE [dbo].[Shipments] drop  CONSTRAINT [UQ_Tenant_ShipmentNumber_Shipments]");
			Sql(@"ALTER TABLE [dbo].[Shipments] drop  CONSTRAINT [UQ_ComputedForwarderShipmentNumber_Tenant]");
			AlterColumn("dbo.Quotes", "QuoteNumber", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.Tickets", "ShipmentNumber", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Tickets", "QuoteNumber", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Shipments", "ShipmentNumber", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.Shipments", "ForwarderShipmentNumber", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Shipments", "CustomerShipmentNumber", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Shipments", "ComputedForwarderShipmentNumber", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.ShipmentMasterDatas", "MasterShipmentNumber", c => c.String(maxLength: 20, unicode: false));
			Sql(@"ALTER TABLE [dbo].[Quotes] ADD  CONSTRAINT [UQ_Tenant_QuoteNumber_Quotes] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[QuoteNumber] ASC
)");
			Sql(@"ALTER TABLE [dbo].[Shipments] ADD  CONSTRAINT [UQ_Tenant_ShipmentNumber_Shipments] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[ShipmentNumber] ASC
)");

			Sql(@"ALTER TABLE [dbo].[Shipments] ADD  CONSTRAINT [UQ_ComputedForwarderShipmentNumber_Tenant] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[ComputedForwarderShipmentNumber] ASC
)");
		}
        
        public override void Down()
        {
            AlterColumn("dbo.ShipmentMasterDatas", "MasterShipmentNumber", c => c.String(maxLength: 25, unicode: false));
            AlterColumn("dbo.Shipments", "ComputedForwarderShipmentNumber", c => c.String(maxLength: 25, unicode: false));
            AlterColumn("dbo.Shipments", "CustomerShipmentNumber", c => c.String(maxLength: 25, unicode: false));
            AlterColumn("dbo.Shipments", "ForwarderShipmentNumber", c => c.String(maxLength: 25, unicode: false));
            AlterColumn("dbo.Shipments", "ShipmentNumber", c => c.String(nullable: false, maxLength: 25, unicode: false));
            AlterColumn("dbo.Tickets", "QuoteNumber", c => c.String(maxLength: 25, unicode: false));
            AlterColumn("dbo.Tickets", "ShipmentNumber", c => c.String(maxLength: 25, unicode: false));
            AlterColumn("dbo.Quotes", "QuoteNumber", c => c.String(nullable: false, maxLength: 25, unicode: false));
        }
    }
}
