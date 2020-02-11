namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStorageFreeDaysColumns : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cards", "StorageFreeDays", c => c.Int());
            AlterColumn("dbo.Shipments", "WarehouseStorageFreeDays", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Shipments", "WarehouseStorageFreeDays", c => c.Int(nullable: false));
            AlterColumn("dbo.Cards", "StorageFreeDays", c => c.Int(nullable: false));
        }
    }
}
