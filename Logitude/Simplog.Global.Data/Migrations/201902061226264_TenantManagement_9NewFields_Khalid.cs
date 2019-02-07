namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TenantManagement_9NewFields_Khalid : DbMigration
    {
        public override void Up()
        {

            AddColumn("dbo.TenantManagements", "BluesnapCRMContractId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TenantManagements", "BluesnapEAWBContractId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TenantManagements", "BluesnapEAWBSContractId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TenantManagements", "BluesnapOneTimeContractId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TenantManagements", "BluesnapContractQTY", c => c.Int(nullable: false));
            AddColumn("dbo.TenantManagements", "BluesnapCRMContractQTY", c => c.Int(nullable: false));
            AddColumn("dbo.TenantManagements", "BluesnapEAWBContractQTY", c => c.Int(nullable: false));
            AddColumn("dbo.TenantManagements", "BluesnapEAWBSContractQTY", c => c.Int(nullable: false));
            AddColumn("dbo.TenantManagements", "BluesnapOneTimeContractQTY", c => c.Int(nullable: false));
            CreateIndex("dbo.TenantManagements", "BluesnapCRMContractId");
            CreateIndex("dbo.TenantManagements", "BluesnapEAWBContractId");
            CreateIndex("dbo.TenantManagements", "BluesnapEAWBSContractId");
            CreateIndex("dbo.TenantManagements", "BluesnapOneTimeContractId");
            AddForeignKey("dbo.TenantManagements", "BluesnapCRMContractId", "dbo.BluesnapContracts", "Id");
            AddForeignKey("dbo.TenantManagements", "BluesnapEAWBContractId", "dbo.BluesnapContracts", "Id");
            AddForeignKey("dbo.TenantManagements", "BluesnapEAWBSContractId", "dbo.BluesnapContracts", "Id");
            AddForeignKey("dbo.TenantManagements", "BluesnapOneTimeContractId", "dbo.BluesnapContracts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TenantManagements", "BluesnapOneTimeContractId", "dbo.BluesnapContracts");
            DropForeignKey("dbo.TenantManagements", "BluesnapEAWBSContractId", "dbo.BluesnapContracts");
            DropForeignKey("dbo.TenantManagements", "BluesnapEAWBContractId", "dbo.BluesnapContracts");
            DropForeignKey("dbo.TenantManagements", "BluesnapCRMContractId", "dbo.BluesnapContracts");
            DropIndex("dbo.TenantManagements", new[] { "BluesnapOneTimeContractId" });
            DropIndex("dbo.TenantManagements", new[] { "BluesnapEAWBSContractId" });
            DropIndex("dbo.TenantManagements", new[] { "BluesnapEAWBContractId" });
            DropIndex("dbo.TenantManagements", new[] { "BluesnapCRMContractId" });
            DropColumn("dbo.TenantManagements", "BluesnapOneTimeContractQTY");
            DropColumn("dbo.TenantManagements", "BluesnapEAWBSContractQTY");
            DropColumn("dbo.TenantManagements", "BluesnapEAWBContractQTY");
            DropColumn("dbo.TenantManagements", "BluesnapCRMContractQTY");
            DropColumn("dbo.TenantManagements", "BluesnapContractQTY");
            DropColumn("dbo.TenantManagements", "BluesnapOneTimeContractId");
            DropColumn("dbo.TenantManagements", "BluesnapEAWBSContractId");
            DropColumn("dbo.TenantManagements", "BluesnapEAWBContractId");
            DropColumn("dbo.TenantManagements", "BluesnapCRMContractId");
        }
    }
}
