namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nawras_Delete7FieldsFromTenantTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tenants", "LogBoxAdminUserId", "dbo.Contacts");
            DropIndex("dbo.Tenants", new[] { "LogBoxAdminUserId" });
            DropColumn("dbo.Tenants", "IsDocumentsArchive");
            DropColumn("dbo.Tenants", "CustomerTenantShareImportFile");
            DropColumn("dbo.Tenants", "LogBoxAdminUserId");
            DropColumn("dbo.Tenants", "DocumentShareAsDefault");
            DropColumn("dbo.Tenants", "StockTypeCode");
            DropColumn("dbo.Tenants", "AutoArchiveOnInvoice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tenants", "AutoArchiveOnInvoice", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "StockTypeCode", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tenants", "DocumentShareAsDefault", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "LogBoxAdminUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tenants", "CustomerTenantShareImportFile", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "IsDocumentsArchive", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Tenants", "LogBoxAdminUserId");
            AddForeignKey("dbo.Tenants", "LogBoxAdminUserId", "dbo.Contacts", "Id");
        }
    }
}
