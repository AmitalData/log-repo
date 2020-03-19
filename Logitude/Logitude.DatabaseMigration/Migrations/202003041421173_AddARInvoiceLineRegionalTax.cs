namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddARInvoiceLineRegionalTax : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ARInvoiceLines", "IsRegionalTax", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ARInvoiceLines", "IsRegionalTax");
        }
    }
}
