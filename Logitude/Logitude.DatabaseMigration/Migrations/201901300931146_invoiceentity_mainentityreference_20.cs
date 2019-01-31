namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class invoiceentity_mainentityreference_20 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.APInvoiceEntities", "EntityReference", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.ARInvoiceEntities", "EntityReference", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ARInvoiceEntities", "EntityReference", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.APInvoiceEntities", "EntityReference", c => c.String(maxLength: 15, unicode: false));
        }
    }
}
