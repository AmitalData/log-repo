namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _ChangeCatDWFieldRelationToCode_Rabaia : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.DWObjectFieldCategories", name: "DWObjectFieldId", newName: "DWObjectFieldCode");
            RenameIndex(table: "dbo.DWObjectFieldCategories", name: "IX_DWObjectFieldId", newName: "IX_DWObjectFieldCode");
            //AddColumn("dbo.Features", "ToggleCode", c => c.String(maxLength: 3, unicode: false));
            //AlterColumn("dbo.Quotes", "QuoteNumber", c => c.String(nullable: false, maxLength: 20, unicode: false));
            //AlterColumn("dbo.Tickets", "ShipmentNumber", c => c.String(maxLength: 20, unicode: false));
            //AlterColumn("dbo.Tickets", "QuoteNumber", c => c.String(maxLength: 20, unicode: false));
            //AlterColumn("dbo.APILogs", "Refrence", c => c.String(maxLength: 40, unicode: false));
            //AlterColumn("dbo.Shipments", "ShipmentNumber", c => c.String(nullable: false, maxLength: 20, unicode: false));
            //AlterColumn("dbo.Shipments", "ForwarderShipmentNumber", c => c.String(maxLength: 20, unicode: false));
            //AlterColumn("dbo.Shipments", "CustomerShipmentNumber", c => c.String(maxLength: 20, unicode: false));
            //AlterColumn("dbo.Shipments", "ComputedForwarderShipmentNumber", c => c.String(maxLength: 20, unicode: false));
            //AlterColumn("dbo.ShipmentMasterDatas", "MasterShipmentNumber", c => c.String(maxLength: 20, unicode: false));
            //AlterColumn("dbo.CounterDefinitions", "Prefix", c => c.String(maxLength: 20, unicode: false));
            //AlterColumn("dbo.CounterDefinitions", "Suffix", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CounterDefinitions", "Suffix", c => c.String(maxLength: 10, unicode: false));
            AlterColumn("dbo.CounterDefinitions", "Prefix", c => c.String(maxLength: 10, unicode: false));
            AlterColumn("dbo.ShipmentMasterDatas", "MasterShipmentNumber", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Shipments", "ComputedForwarderShipmentNumber", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Shipments", "CustomerShipmentNumber", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Shipments", "ForwarderShipmentNumber", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Shipments", "ShipmentNumber", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.APILogs", "Refrence", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Tickets", "QuoteNumber", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Tickets", "ShipmentNumber", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Quotes", "QuoteNumber", c => c.String(nullable: false, maxLength: 15, unicode: false));
            DropColumn("dbo.Features", "ToggleCode");
            RenameIndex(table: "dbo.DWObjectFieldCategories", name: "IX_DWObjectFieldCode", newName: "IX_DWObjectFieldId");
            RenameColumn(table: "dbo.DWObjectFieldCategories", name: "DWObjectFieldCode", newName: "DWObjectFieldId");
        }
    }
}
