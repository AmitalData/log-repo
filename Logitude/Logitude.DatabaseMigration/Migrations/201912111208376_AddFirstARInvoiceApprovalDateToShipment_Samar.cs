namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFirstARInvoiceApprovalDateToShipment_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "FirstARInvoiceApprovalDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "FirstARInvoiceApprovalDate");
        }
    }
}
