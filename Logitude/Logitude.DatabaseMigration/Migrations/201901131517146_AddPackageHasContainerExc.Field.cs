namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPackageHasContainerExcField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentPackages", "HasContainerException", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShipmentPackages", "HasContainerException");
        }
    }
}
