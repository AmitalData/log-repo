namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Abed_AddBatchServicesDefinitionMapToGlobalContext : DbMigration
    {
        public override void Up()
        {
            Sql("declare @sql nvarchar(2000) " +
                "declare @fkName varchar(500) " +
                "set @fkName = (select f.name from sys.foreign_keys as f " +
                "inner join sys.foreign_key_columns as fc on f.object_id = fc.constraint_object_id " +
                "where f.parent_object_id = object_id('[dbo].[BatchServicesDefinitionMods]') and col_name(fc.parent_object_id, fc.parent_column_id) = 'Code') " +
                "set @sql = 'alter table [dbo].[BatchServicesDefinitionMods] drop constraint [' + @fkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[BatchServicesDefinitionMods]') and col_name(i.object_id, ic.column_id) = 'Code') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[BatchServicesDefinitionMods]' " +
                "exec(@sql)");


            //DropColumn("dbo.TenantManagements", "BluesnapContractId");
            //RenameColumn(table: "dbo.TenantManagements", name: "BluesnapInttraStockContractId", newName: "BluesnapContractId");
            //RenameIndex(table: "dbo.TenantManagements", name: "IX_BluesnapInttraStockContractId", newName: "IX_BluesnapContractId");


            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'BatchServicesDefinitionMods') " +
                "set @sql = 'alter table [dbo].[BatchServicesDefinitionMods] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'BatchServicesDefinitions') " +
                "set @sql = 'alter table [dbo].[BatchServicesDefinitions] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");


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
