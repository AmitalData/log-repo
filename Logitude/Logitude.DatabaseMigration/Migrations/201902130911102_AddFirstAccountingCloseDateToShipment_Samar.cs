namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFirstAccountingCloseDateToShipment_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "FirstAccountingCloseDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "FirstAccountingCloseDate");
        }
    }
}
