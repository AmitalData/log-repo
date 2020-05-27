namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddIsUsedToWarehouseRelease : DbMigration
    {
        public override void Up()
        {
    
            AddColumn("dbo.WarehouseReleases", "IsUsed", c => c.Boolean(nullable: false));
            DropColumn("dbo.WarehouseReleasePackages", "IsUsed");
        }
        
        public override void Down()
        {
    
            DropColumn("dbo.WarehouseReleases", "IsUsed");
            AddColumn("dbo.WarehouseReleasePackages", "IsUsed", c => c.Boolean(nullable: false));
        }
    }
}
