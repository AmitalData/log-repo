namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddARInvoiceConcurrency : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ARInvoices", "ConcurrencyGUID", c => c.String(maxLength: 40, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ARInvoices", "ConcurrencyGUID");
        }
    }
}
