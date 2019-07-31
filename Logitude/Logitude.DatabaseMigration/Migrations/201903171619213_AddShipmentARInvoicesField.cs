namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShipmentARInvoicesField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "ARInvoices", c => c.String(maxLength: 1000, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "ARInvoices");
        }
    }
}
