namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddIsUsedToWarehouseReleasePackage : DbMigration
    {
        public override void Up()
        {

            AddColumn("dbo.WarehouseReleasePackages", "IsUsed", c => c.Boolean(nullable: false));

        }
        
        public override void Down()
        {

            DropColumn("dbo.WarehouseReleasePackages", "IsUsed");
    
        }
    }
}
