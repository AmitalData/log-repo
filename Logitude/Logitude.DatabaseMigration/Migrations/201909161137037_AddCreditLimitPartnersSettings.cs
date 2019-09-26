namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreditLimitPartnersSettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CreditLimitSettings", "CustomersShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "AgentsShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "ShipperConsigneeShipmentBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "CustomsAgentsShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "ShippingAgentsShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "AirlinesShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "ShippingLinesShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "TruckersShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "VendorsShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "WarehousesShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "CustomersInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "AgentsInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "ShipperConsigneeInvoiceBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "CustomsAgentsInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "ShippingAgentsInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "AirlinesInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "ShippingLinesInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "TruckersInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "VendorsInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "WarehousesInvoicesBlock", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CreditLimitSettings", "WarehousesInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "VendorsInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "TruckersInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "ShippingLinesInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "AirlinesInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "ShippingAgentsInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "CustomsAgentsInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "ShipperConsigneeInvoiceBlock");
            DropColumn("dbo.CreditLimitSettings", "AgentsInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "CustomersInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "WarehousesShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "VendorsShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "TruckersShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "ShippingLinesShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "AirlinesShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "ShippingAgentsShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "CustomsAgentsShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "ShipperConsigneeShipmentBlock");
            DropColumn("dbo.CreditLimitSettings", "AgentsShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "CustomersShipmentsBlock");
        }
    }
}
