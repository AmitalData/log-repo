namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotesFieldToARInvoiceStock_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ARInvoiceStocks", "Notes", c => c.String(maxLength: 250, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ARInvoiceStocks", "Notes");
        }
    }
}
