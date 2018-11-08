namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTwentyCustomeFieldsOnShipment_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "Field21", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field22", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field23", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field24", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field25", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field26", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field27", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field28", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field29", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field30", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field31", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field32", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field33", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field34", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field35", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field36", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field37", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field38", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field39", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "Field40", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "Field40");
            DropColumn("dbo.Shipments", "Field39");
            DropColumn("dbo.Shipments", "Field38");
            DropColumn("dbo.Shipments", "Field37");
            DropColumn("dbo.Shipments", "Field36");
            DropColumn("dbo.Shipments", "Field35");
            DropColumn("dbo.Shipments", "Field34");
            DropColumn("dbo.Shipments", "Field33");
            DropColumn("dbo.Shipments", "Field32");
            DropColumn("dbo.Shipments", "Field31");
            DropColumn("dbo.Shipments", "Field30");
            DropColumn("dbo.Shipments", "Field29");
            DropColumn("dbo.Shipments", "Field28");
            DropColumn("dbo.Shipments", "Field27");
            DropColumn("dbo.Shipments", "Field26");
            DropColumn("dbo.Shipments", "Field25");
            DropColumn("dbo.Shipments", "Field24");
            DropColumn("dbo.Shipments", "Field23");
            DropColumn("dbo.Shipments", "Field22");
            DropColumn("dbo.Shipments", "Field21");
        }
    }
}
