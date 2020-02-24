namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSLACFieldToShipment_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "SLAC", c => c.String(maxLength: 5, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "SLAC");
        }
    }
}
