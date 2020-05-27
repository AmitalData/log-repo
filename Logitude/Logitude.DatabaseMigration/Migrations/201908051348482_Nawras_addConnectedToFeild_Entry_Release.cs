namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nawras_addConnectedToFeild_Entry_Release : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WarehouseEntries", "ConnectedTo", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.WarehouseReleases", "ConnectedTo", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WarehouseReleases", "ConnectedTo");
            DropColumn("dbo.WarehouseEntries", "ConnectedTo");
        }
    }
}
