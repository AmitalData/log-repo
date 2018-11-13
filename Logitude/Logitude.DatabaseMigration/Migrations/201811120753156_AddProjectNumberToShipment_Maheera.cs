namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectNumberToShipment_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "ProjectNumber", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "ProjectNumber");
        }
    }
}
