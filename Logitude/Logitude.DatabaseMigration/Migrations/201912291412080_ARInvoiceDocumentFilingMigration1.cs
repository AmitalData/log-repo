namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ARInvoiceDocumentFilingMigration1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ARInvoices", "DocumentFilingId", c => c.String(maxLength: 40, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ARInvoices", "DocumentFilingId", c => c.String());
        }
    }
}
