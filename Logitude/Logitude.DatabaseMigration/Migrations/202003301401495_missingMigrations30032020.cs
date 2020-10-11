namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingMigrations30032020 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
               "dbo.LogBoxTenantSettings",
               c => new
               {
                   Id = c.Int(nullable: false),
                   IsDocumentsArchive = c.Boolean(nullable: false),
                   CustomerTenantShareImportFile = c.Boolean(nullable: false),
                   LogBoxAdminUserId = c.String(maxLength: 15, unicode: false),
                   DocumentShareAsDefault = c.Boolean(nullable: false),
                   StockTypeCode = c.String(maxLength: 15, unicode: false),
                   AutoArchiveOnInvoice = c.Boolean(nullable: false),
               })
               .PrimaryKey(t => t.Id)
               .ForeignKey("dbo.Contacts", t => t.LogBoxAdminUserId)
               .ForeignKey("dbo.Tenants", t => t.Id)
               .Index(t => t.Id)
               .Index(t => t.LogBoxAdminUserId);
        }
        
        public override void Down()
        {
        }
    }
}
