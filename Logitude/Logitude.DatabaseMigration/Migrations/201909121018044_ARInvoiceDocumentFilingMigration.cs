namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ARInvoiceDocumentFilingMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ARInvoices", "DocumentFilingId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ARInvoices", "DocumentFilingId");
        }
    }
}
