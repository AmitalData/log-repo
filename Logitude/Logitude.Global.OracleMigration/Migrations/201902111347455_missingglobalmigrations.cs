namespace Logitude.Global.OracleMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingglobalmigrations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TenantManagements", "BluesnapContractCode", "dbo.BluesnapContracts");
            DropIndex("dbo.TenantManagements", new[] { "BluesnapContractCode" });
            RenameColumn(table: "dbo.TenantManagements", name: "BluesnapContractCode", newName: "BluesnapOneTimeContractId");
            DropPrimaryKey("dbo.BluesnapContracts");
            CreateTable(
                "dbo.BluesnapContractTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        SearchFields = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            AddColumn("dbo.TenantManagements", "BluesnapContractId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TenantManagements", "BluesnapCRMContractId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TenantManagements", "BluesnapEAWBContractId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TenantManagements", "BluesnapEAWBSContractId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TenantManagements", "BluesnapContractQTY", c => c.Int(nullable: false));
            AddColumn("dbo.TenantManagements", "BluesnapCRMContractQTY", c => c.Int(nullable: false));
            AddColumn("dbo.TenantManagements", "BluesnapEAWBContractQTY", c => c.Int(nullable: false));
            AddColumn("dbo.TenantManagements", "BluesnapEAWBSContractQTY", c => c.Int(nullable: false));
            AddColumn("dbo.TenantManagements", "BluesnapOneTimeContractQTY", c => c.Int(nullable: false));
            AddColumn("dbo.BluesnapContracts", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AddColumn("dbo.BluesnapContracts", "BluesnapContractTypeCode", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AnalyzeQueues", "EntityReference", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.TenantManagements", "BluesnapOneTimeContractId", c => c.String(maxLength: 15, unicode: false));
            AddPrimaryKey("dbo.BluesnapContracts", "Id");
            CreateIndex("dbo.TenantManagements", "BluesnapCRMContractId");
            CreateIndex("dbo.TenantManagements", "BluesnapEAWBContractId");
            CreateIndex("dbo.TenantManagements", "BluesnapEAWBSContractId");
            CreateIndex("dbo.TenantManagements", "BluesnapOneTimeContractId");
            CreateIndex("dbo.BluesnapContracts", "BluesnapContractTypeCode");
            AddForeignKey("dbo.BluesnapContracts", "BluesnapContractTypeCode", "dbo.BluesnapContractTypes", "Code", cascadeDelete: true);
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
            DropForeignKey("dbo.BluesnapContracts", "BluesnapContractTypeCode", "dbo.BluesnapContractTypes");
            DropIndex("dbo.BluesnapContracts", new[] { "BluesnapContractTypeCode" });
            DropIndex("dbo.TenantManagements", new[] { "BluesnapOneTimeContractId" });
            DropIndex("dbo.TenantManagements", new[] { "BluesnapEAWBSContractId" });
            DropIndex("dbo.TenantManagements", new[] { "BluesnapEAWBContractId" });
            DropIndex("dbo.TenantManagements", new[] { "BluesnapCRMContractId" });
            DropPrimaryKey("dbo.BluesnapContracts");
            AlterColumn("dbo.TenantManagements", "BluesnapOneTimeContractId", c => c.String(maxLength: 10, unicode: false));
            AlterColumn("dbo.AnalyzeQueues", "EntityReference", c => c.String(maxLength: 15, unicode: false));
            DropColumn("dbo.BluesnapContracts", "BluesnapContractTypeCode");
            DropColumn("dbo.BluesnapContracts", "Id");
            DropColumn("dbo.TenantManagements", "BluesnapOneTimeContractQTY");
            DropColumn("dbo.TenantManagements", "BluesnapEAWBSContractQTY");
            DropColumn("dbo.TenantManagements", "BluesnapEAWBContractQTY");
            DropColumn("dbo.TenantManagements", "BluesnapCRMContractQTY");
            DropColumn("dbo.TenantManagements", "BluesnapContractQTY");
            DropColumn("dbo.TenantManagements", "BluesnapEAWBSContractId");
            DropColumn("dbo.TenantManagements", "BluesnapEAWBContractId");
            DropColumn("dbo.TenantManagements", "BluesnapCRMContractId");
            DropColumn("dbo.TenantManagements", "BluesnapContractId");
            DropTable("dbo.BluesnapContractTypes");
            AddPrimaryKey("dbo.BluesnapContracts", "Code");
            RenameColumn(table: "dbo.TenantManagements", name: "BluesnapOneTimeContractId", newName: "BluesnapContractCode");
            CreateIndex("dbo.TenantManagements", "BluesnapContractCode");
            AddForeignKey("dbo.TenantManagements", "BluesnapContractCode", "dbo.BluesnapContracts", "Code");
        }
    }
}
