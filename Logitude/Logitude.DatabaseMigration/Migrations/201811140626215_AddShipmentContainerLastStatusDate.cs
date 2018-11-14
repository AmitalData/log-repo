namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShipmentContainerLastStatusDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "ContainerLastStatusDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "ContainerLastStatusDate");
        }
    }
}
