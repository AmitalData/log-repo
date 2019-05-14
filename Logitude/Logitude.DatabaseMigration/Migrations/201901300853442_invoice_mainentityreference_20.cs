namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class invoice_mainentityreference_20 : DbMigration
    {
        public override void Up()
        {

			AlterColumn("dbo.APInvoices", "MainEntityReference", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.ARInvoices", "MainEntityReference", c => c.String(maxLength: 20, unicode: false));
           
        }
        
        public override void Down()
        {
             
            AlterColumn("dbo.ARInvoices", "MainEntityReference", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.APInvoices", "MainEntityReference", c => c.String(maxLength: 15, unicode: false));
             
        }
    }
}
