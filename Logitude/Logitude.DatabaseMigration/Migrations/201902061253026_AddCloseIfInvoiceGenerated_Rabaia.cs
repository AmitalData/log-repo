namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCloseIfInvoiceGenerated_Rabaia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tenants", "AutoArchiveOnInvoice", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tenants", "AutoArchiveOnInvoice");
        }
    }
}
