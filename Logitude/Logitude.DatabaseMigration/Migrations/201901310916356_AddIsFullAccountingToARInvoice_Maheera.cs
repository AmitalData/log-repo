namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsFullAccountingToARInvoice_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ARInvoices", "IsFullAccounting", c => c.Boolean(nullable: false));
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.ARInvoices", "IsFullAccounting");
        }
    }
}
