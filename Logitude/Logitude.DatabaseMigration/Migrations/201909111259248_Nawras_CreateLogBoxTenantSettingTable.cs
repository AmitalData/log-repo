namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nawras_CreateLogBoxTenantSettingTable : DbMigration
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



            Sql("insert into LogBoxTenantSettings (Id,AutoArchiveOnInvoice,StockTypeCode,DocumentShareAsDefault,LogBoxAdminUserId,IsDocumentsArchive,CustomerTenantShareImportFile) select Tenants.Id,Tenants.AutoArchiveOnInvoice,Tenants.StockTypeCode,Tenants.DocumentShareAsDefault,Tenants.LogBoxAdminUserId,Tenants.IsDocumentsArchive,Tenants.CustomerTenantShareImportFile From Tenants");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogBoxTenantSettings", "Id", "dbo.Tenants");
            DropForeignKey("dbo.LogBoxTenantSettings", "LogBoxAdminUserId", "dbo.Contacts");
            DropIndex("dbo.LogBoxTenantSettings", new[] { "LogBoxAdminUserId" });
            DropIndex("dbo.LogBoxTenantSettings", new[] { "Id" });
            DropTable("dbo.LogBoxTenantSettings");
        }
    }
}
