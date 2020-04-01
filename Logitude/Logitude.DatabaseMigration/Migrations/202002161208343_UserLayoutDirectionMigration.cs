namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserLayoutDirectionMigration : DbMigration
    {
        public override void Up()
        {
         //   AddColumn("dbo.Cards", "StorageFreeDays", c => c.Int());
            AddColumn("dbo.Users", "LayoutDirection", c => c.String(maxLength: 3, unicode: false , defaultValue:"ltr") );
          //  AddColumn("dbo.Tickets", "LastCorrespondence", c => c.String(maxLength: 4000));
          //  AddColumn("dbo.Shipments", "GrossWeightPerStorageDays", c => c.Double());
          //  AddColumn("dbo.Shipments", "WarehouseStorageFreeDays", c => c.Int());
            //AddColumn("dbo.CustomsTransferHeaders", "ShipmentNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomsTransferHeaders", "ShipmentNumber");
            DropColumn("dbo.Shipments", "WarehouseStorageFreeDays");
            DropColumn("dbo.Shipments", "GrossWeightPerStorageDays");
            DropColumn("dbo.Tickets", "LastCorrespondence");
            DropColumn("dbo.Users", "LayoutDirection");
            DropColumn("dbo.Cards", "StorageFreeDays");
        }
    }
}
