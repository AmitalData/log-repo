namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetInvoiceIdNotRequiredOnStockLineLevel_Samar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ARInvoiceStockLines", "ARInvoiceId", c => c.String(maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ARInvoiceStockLines", "ARInvoiceId", c => c.String(nullable: false, maxLength: 15, unicode: false));
        }
    }
}
