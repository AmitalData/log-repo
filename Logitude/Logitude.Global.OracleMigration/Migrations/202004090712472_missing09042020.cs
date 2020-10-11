namespace Logitude.Global.OracleMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missing09042020 : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.TenantManagements", "BluesnapContractId");
            //RenameColumn(table: "dbo.TenantManagements", name: "BluesnapInttraStockContractId", newName: "BluesnapContractId");
            //RenameIndex(table: "dbo.TenantManagements", name: "IX_BluesnapInttraStockContractId", newName: "IX_BluesnapContractId");
            CreateTable(
                "dbo.BluesnapTransactions",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Tenant = c.Int(nullable: false),
                    CreateDate = c.DateTime(nullable: false, precision: 7),
                    TransactionDate = c.DateTime(precision: 7),
                    DocumentId = c.String(),
                    LogitudeAmital = c.String(),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.TenantManagements", "TotalPrice", c => c.Double());
            AddColumn("dbo.TenantManagements", "SupportDomain", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.TenantManagements", "TotalNumberOfUsers", c => c.Int());
            AddColumn("dbo.TenantManagements", "TotalFreeUsers", c => c.Int());
            AddColumn("dbo.TenantManagements", "AveragePrice", c => c.Double());
            AddColumn("dbo.TenantManagements", "TotalPaymentamount", c => c.Double());
            AddColumn("dbo.BatchServicesDefinitions", "QueueDefinitionCode", c => c.String());
            AddColumn("dbo.Settings", "QBOOAuthDefault", c => c.Int(nullable: false));
            AddColumn("dbo.Settings", "QBOClientID", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Settings", "QBOClientSecret", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.TenantManagementLicenses", "FreeUsers", c => c.Int());
            AddColumn("dbo.TenantManagementLicenses", "Price", c => c.Double());
            AddColumn("dbo.TenantManagementLicenses", "TotalPrice", c => c.Double());
            AddColumn("dbo.WebhookKeys", "SearchFields", c => c.String(maxLength: 1000));
            //AlterColumn("dbo.TenantManagements", "CountryName", c => c.String(maxLength: 120, unicode: false));
            AlterColumn("dbo.Settings", "StorageAccountKey", c => c.String(nullable: false, maxLength: 1000, unicode: false));
            //CreateIndex("dbo.TenantManagements", "BluesnapInttraStockContractId");
            DropColumn("dbo.Settings", "IsFullBuildDWRunning");
            DropColumn("dbo.Settings", "IsIncrementalDWRunning");
            DropColumn("dbo.Settings", "DWNextRunTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Settings", "DWNextRunTime", c => c.DateTime(precision: 7));
            AddColumn("dbo.Settings", "IsIncrementalDWRunning", c => c.Boolean(nullable: false));
            AddColumn("dbo.Settings", "IsFullBuildDWRunning", c => c.Boolean(nullable: false));
            DropIndex("dbo.TenantManagements", new[] { "BluesnapInttraStockContractId" });
            AlterColumn("dbo.Settings", "StorageAccountKey", c => c.String());
            AlterColumn("dbo.TenantManagements", "CountryName", c => c.String());
            DropColumn("dbo.WebhookKeys", "SearchFields");
            DropColumn("dbo.TenantManagementLicenses", "TotalPrice");
            DropColumn("dbo.TenantManagementLicenses", "Price");
            DropColumn("dbo.TenantManagementLicenses", "FreeUsers");
            DropColumn("dbo.Settings", "QBOClientSecret");
            DropColumn("dbo.Settings", "QBOClientID");
            DropColumn("dbo.Settings", "QBOOAuthDefault");
            DropColumn("dbo.BatchServicesDefinitions", "QueueDefinitionCode");
            DropColumn("dbo.TenantManagements", "TotalPaymentamount");
            DropColumn("dbo.TenantManagements", "AveragePrice");
            DropColumn("dbo.TenantManagements", "TotalFreeUsers");
            DropColumn("dbo.TenantManagements", "TotalNumberOfUsers");
            DropColumn("dbo.TenantManagements", "SupportDomain");
            DropColumn("dbo.TenantManagements", "TotalPrice");
            DropTable("dbo.BluesnapTransactions");
            RenameIndex(table: "dbo.TenantManagements", name: "IX_BluesnapContractId", newName: "IX_BluesnapInttraStockContractId");
            RenameColumn(table: "dbo.TenantManagements", name: "BluesnapContractId", newName: "BluesnapInttraStockContractId");
            AddColumn("dbo.TenantManagements", "BluesnapContractId", c => c.String(maxLength: 15, unicode: false));
        }
    }
}
