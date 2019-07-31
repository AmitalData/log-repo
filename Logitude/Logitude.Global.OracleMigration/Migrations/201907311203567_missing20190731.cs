namespace Logitude.Global.OracleMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missing20190731 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TenantManagements", name: "BluesnapOneTimeContractId", newName: "BluesnapInttraStockContractId");
            //RenameIndex(table: "dbo.TenantManagements", name: "IX_BluesnapOneTimeContractId", newName: "IX_BluesnapInttraStockContractId");
            AddColumn("dbo.AnalyzeQueues", "Log", c => c.String(maxLength: 500, unicode: false));
            AddColumn("dbo.TenantManagements", "BluesnapOneTimeContract", c => c.String(maxLength: 40, unicode: false));
            AddColumn("dbo.TenantManagements", "BluesnapInttraStockContractQTY", c => c.Int());
            AddColumn("dbo.TenantManagements", "MainAdditionalPackageApplied", c => c.Boolean(nullable: false));
            AddColumn("dbo.GlobalDBs", "SecondaryAzureDBConnection", c => c.String(nullable: true, maxLength: 512));
            AddColumn("dbo.GlobalDBs", "IsBlocking", c => c.Boolean(nullable: false));
            AddColumn("dbo.Settings", "ChampTestAPIURL", c => c.String(nullable: true, maxLength: 1000, unicode: false));
            AddColumn("dbo.Settings", "ChampTestAPIPassword", c => c.String(nullable: true, maxLength: 40, unicode: false));
            AddColumn("dbo.Settings", "ChampProdAPIURL", c => c.String(nullable: true, maxLength: 1000, unicode: false));
            AddColumn("dbo.Settings", "ChampProdAPIPassword", c => c.String(nullable: true, maxLength: 40, unicode: false));
            AddColumn("dbo.Settings", "ReleaseNotesURL", c => c.String(maxLength: 600, unicode: false));
            AddColumn("dbo.Settings", "CPUIntensiveWebServicesURL", c => c.String(maxLength: 1000, unicode: false));
            AlterColumn("dbo.TenantManagements", "BluesnapContractQTY", c => c.Int());
            AlterColumn("dbo.TenantManagements", "BluesnapCRMContractQTY", c => c.Int());
            AlterColumn("dbo.TenantManagements", "BluesnapEAWBContractQTY", c => c.Int());
            AlterColumn("dbo.TenantManagements", "BluesnapEAWBSContractQTY", c => c.Int());
            AlterColumn("dbo.TenantManagements", "BluesnapOneTimeContractQTY", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TenantManagements", "BluesnapOneTimeContractQTY", c => c.Int(nullable: false));
            AlterColumn("dbo.TenantManagements", "BluesnapEAWBSContractQTY", c => c.Int(nullable: false));
            AlterColumn("dbo.TenantManagements", "BluesnapEAWBContractQTY", c => c.Int(nullable: false));
            AlterColumn("dbo.TenantManagements", "BluesnapCRMContractQTY", c => c.Int(nullable: false));
            AlterColumn("dbo.TenantManagements", "BluesnapContractQTY", c => c.Int(nullable: false));
            DropColumn("dbo.Settings", "CPUIntensiveWebServicesURL");
            DropColumn("dbo.Settings", "ReleaseNotesURL");
            DropColumn("dbo.Settings", "ChampProdAPIPassword");
            DropColumn("dbo.Settings", "ChampProdAPIURL");
            DropColumn("dbo.Settings", "ChampTestAPIPassword");
            DropColumn("dbo.Settings", "ChampTestAPIURL");
            DropColumn("dbo.GlobalDBs", "IsBlocking");
            DropColumn("dbo.GlobalDBs", "SecondaryAzureDBConnection");
            DropColumn("dbo.TenantManagements", "MainAdditionalPackageApplied");
            DropColumn("dbo.TenantManagements", "BluesnapInttraStockContractQTY");
            DropColumn("dbo.TenantManagements", "BluesnapOneTimeContract");
            DropColumn("dbo.AnalyzeQueues", "Log");
            RenameIndex(table: "dbo.TenantManagements", name: "IX_BluesnapInttraStockContractId", newName: "IX_BluesnapOneTimeContractId");
            RenameColumn(table: "dbo.TenantManagements", name: "BluesnapInttraStockContractId", newName: "BluesnapOneTimeContractId");
        }
    }
}
