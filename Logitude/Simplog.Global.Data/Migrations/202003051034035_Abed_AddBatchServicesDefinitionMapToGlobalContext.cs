namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Abed_AddBatchServicesDefinitionMapToGlobalContext : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BatchServicesDefinitionMods", "Code", "dbo.BatchServicesDefinitions");
            DropIndex("dbo.BatchServicesDefinitionMods", new[] { "Code" });
            //DropColumn("dbo.TenantManagements", "BluesnapContractId");
            //RenameColumn(table: "dbo.TenantManagements", name: "BluesnapInttraStockContractId", newName: "BluesnapContractId");
            //RenameIndex(table: "dbo.TenantManagements", name: "IX_BluesnapInttraStockContractId", newName: "IX_BluesnapContractId");
            DropPrimaryKey("dbo.BatchServicesDefinitionMods");
            DropPrimaryKey("dbo.BatchServicesDefinitions");
            //AlterColumn("dbo.TenantManagements", "CountryName", c => c.String(maxLength: 120, unicode: false));
            AlterColumn("dbo.BatchServicesDefinitionMods", "Code", c => c.String(nullable: false, maxLength: 40, unicode: false));
            AlterColumn("dbo.BatchServicesDefinitions", "Code", c => c.String(nullable: false, maxLength: 40, unicode: false));
            AlterColumn("dbo.BatchServicesDefinitions", "ClassName", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AlterColumn("dbo.BatchServicesDefinitions", "Parameter1", c => c.String(maxLength: 25, unicode: false));
            AlterColumn("dbo.BatchServicesDefinitions", "Parameter2", c => c.String(maxLength: 25, unicode: false));
            AlterColumn("dbo.BatchServicesDefinitions", "QueueDefinitionCode", c => c.String(maxLength: 200, unicode: false));
            //AlterColumn("dbo.Settings", "StorageAccountKey", c => c.String(nullable: false, maxLength: 1000, unicode: false));
            AddPrimaryKey("dbo.BatchServicesDefinitionMods", "Code");
            AddPrimaryKey("dbo.BatchServicesDefinitions", "Code");
            //CreateIndex("dbo.TenantManagements", "BluesnapInttraStockContractId");
            CreateIndex("dbo.BatchServicesDefinitionMods", "Code");
            AddForeignKey("dbo.BatchServicesDefinitionMods", "Code", "dbo.BatchServicesDefinitions", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BatchServicesDefinitionMods", "Code", "dbo.BatchServicesDefinitions");
            DropIndex("dbo.BatchServicesDefinitionMods", new[] { "Code" });
            //DropIndex("dbo.TenantManagements", new[] { "BluesnapInttraStockContractId" });
            DropPrimaryKey("dbo.BatchServicesDefinitions");
            DropPrimaryKey("dbo.BatchServicesDefinitionMods");
            //AlterColumn("dbo.Settings", "StorageAccountKey", c => c.String(nullable: false));
            AlterColumn("dbo.BatchServicesDefinitions", "QueueDefinitionCode", c => c.String());
            AlterColumn("dbo.BatchServicesDefinitions", "Parameter2", c => c.String());
            AlterColumn("dbo.BatchServicesDefinitions", "Parameter1", c => c.String());
            AlterColumn("dbo.BatchServicesDefinitions", "ClassName", c => c.String());
            AlterColumn("dbo.BatchServicesDefinitions", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.BatchServicesDefinitionMods", "Code", c => c.String(nullable: false, maxLength: 128));
            //AlterColumn("dbo.TenantManagements", "CountryName", c => c.String());
            AddPrimaryKey("dbo.BatchServicesDefinitions", "Code");
            AddPrimaryKey("dbo.BatchServicesDefinitionMods", "Code");
            //RenameIndex(table: "dbo.TenantManagements", name: "IX_BluesnapContractId", newName: "IX_BluesnapInttraStockContractId");
            //RenameColumn(table: "dbo.TenantManagements", name: "BluesnapContractId", newName: "BluesnapInttraStockContractId");
            //AddColumn("dbo.TenantManagements", "BluesnapContractId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.BatchServicesDefinitionMods", "Code");
            AddForeignKey("dbo.BatchServicesDefinitionMods", "Code", "dbo.BatchServicesDefinitions", "Code");
        }
    }
}
