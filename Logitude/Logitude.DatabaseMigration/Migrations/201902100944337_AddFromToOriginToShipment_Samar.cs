namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFromToOriginToShipment_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "From", c => c.String(maxLength: 150, unicode: false));
            AddColumn("dbo.Shipments", "To", c => c.String(maxLength: 150, unicode: false));
            AddColumn("dbo.Shipments", "Origin", c => c.String(maxLength: 150, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "Origin");
            DropColumn("dbo.Shipments", "To");
            DropColumn("dbo.Shipments", "From");
        }
    }
}
