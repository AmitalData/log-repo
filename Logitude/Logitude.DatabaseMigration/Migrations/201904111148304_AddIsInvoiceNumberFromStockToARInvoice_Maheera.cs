namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsInvoiceNumberFromStockToARInvoice_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ARInvoices", "IsInvoiceNumberFromStock", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ARInvoices", "IsInvoiceNumberFromStock");
        }
    }
}
