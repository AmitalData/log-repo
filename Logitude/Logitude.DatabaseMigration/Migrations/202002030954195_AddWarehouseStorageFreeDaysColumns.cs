namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWarehouseStorageFreeDaysColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cards", "StorageFreeDays", c => c.Int(nullable: false));
            AddColumn("dbo.Shipments", "WarehouseStorageFreeDays", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "WarehouseStorageFreeDays");
            DropColumn("dbo.Cards", "StorageFreeDays");
        }
    }
}
