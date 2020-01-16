namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ARinvoiceBillToGLAccountIdMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ARInvoices", "BillToGLAccountId", c => c.String(maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ARInvoices", "BillToGLAccountId");
        }
    }
}
