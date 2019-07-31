namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveField_IsLedgerCreated : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.ObjectTables", "IsTabsHidden", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Quotes", "GrossWeightEdited", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Quotes", "ChargeableWeightEdited", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Shipments", "OrderGrossWeightEdited", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Shipments", "OrderChargeableWeightEdited", c => c.Boolean(nullable: false));
            AddColumn("dbo.Journals", "IsLedgerCreated", c => c.Boolean(nullable: false));
            //AddColumn("dbo.TariffLines", "OriginPortText", c => c.String(maxLength: 20, unicode: false));
            //AddColumn("dbo.TariffLines", "DestinationPortText", c => c.String(maxLength: 20, unicode: false));
            //AddColumn("dbo.TariffLines", "MinPriceText", c => c.String(maxLength: 20, unicode: false));
            //AddColumn("dbo.TariffLines", "Step1PriceText", c => c.String(maxLength: 20, unicode: false));
            //AddColumn("dbo.TariffLines", "Step2PriceText", c => c.String(maxLength: 20, unicode: false));
            //AddColumn("dbo.TariffLines", "Step3PriceText", c => c.String(maxLength: 20, unicode: false));
            //AddColumn("dbo.TariffLines", "Step4PriceText", c => c.String(maxLength: 20, unicode: false));
            //AddColumn("dbo.TariffLines", "Step5PriceText", c => c.String(maxLength: 20, unicode: false));
            //AddColumn("dbo.TariffLines", "Step6PriceText", c => c.String(maxLength: 20, unicode: false));
            //AddColumn("dbo.TariffLines", "Step7PriceText", c => c.String(maxLength: 20, unicode: false));
            //AddColumn("dbo.TariffLines", "Step8PriceText", c => c.String(maxLength: 20, unicode: false));
            //AddColumn("dbo.Tariffs", "TariffNumber", c => c.String(maxLength: 20, unicode: false));
            DropColumn("dbo.JournalMoreDatas", "IsLedgerCreated");
        }

        public override void Down()
        {
            AddColumn("dbo.JournalMoreDatas", "IsLedgerCreated", c => c.Boolean(nullable: false));
            //DropColumn("dbo.Tariffs", "TariffNumber");
            //DropColumn("dbo.TariffLines", "Step8PriceText");
            //DropColumn("dbo.TariffLines", "Step7PriceText");
            //DropColumn("dbo.TariffLines", "Step6PriceText");
            //DropColumn("dbo.TariffLines", "Step5PriceText");
            //DropColumn("dbo.TariffLines", "Step4PriceText");
            //DropColumn("dbo.TariffLines", "Step3PriceText");
            //DropColumn("dbo.TariffLines", "Step2PriceText");
            //DropColumn("dbo.TariffLines", "Step1PriceText");
            //DropColumn("dbo.TariffLines", "MinPriceText");
            //DropColumn("dbo.TariffLines", "DestinationPortText");
            //DropColumn("dbo.TariffLines", "OriginPortText");
            DropColumn("dbo.Journals", "IsLedgerCreated");
            //DropColumn("dbo.Shipments", "OrderChargeableWeightEdited");
            //DropColumn("dbo.Shipments", "OrderGrossWeightEdited");
            //DropColumn("dbo.Quotes", "ChargeableWeightEdited");
            //DropColumn("dbo.Quotes", "GrossWeightEdited");
            //DropColumn("dbo.ObjectTables", "IsTabsHidden");
        }
    }
}
