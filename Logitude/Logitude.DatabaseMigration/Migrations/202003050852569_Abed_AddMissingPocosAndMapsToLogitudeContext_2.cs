namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Abed_AddMissingPocosAndMapsToLogitudeContext_2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DocumentTypeCustomsDatas", newName: "DocumentTypeCustomsData");
            //MoveTable(name: "dbo.DecisionTypes", newSchema: "Customs");
            MoveTable(name: "dbo.DocumentTypeCustomsData", newSchema: "Customs");

            Sql("declare @sql nvarchar(2000) " +
                "declare @fkName varchar(500) " +
                "set @fkName = (select f.name from sys.foreign_keys as f " +
                "inner join sys.foreign_key_columns as fc on f.object_id = fc.constraint_object_id " +
                "where f.parent_object_id = object_id('[dbo].[Customers]') and col_name(fc.parent_object_id, fc.parent_column_id) = 'CustomerSizeId') " +
                "set @sql = 'alter table [dbo].[Customers] drop constraint [' + @fkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @fkName varchar(500) " +
                "set @fkName = (select f.name from sys.foreign_keys as f " +
                "inner join sys.foreign_key_columns as fc on f.object_id = fc.constraint_object_id " +
                "where f.parent_object_id = object_id('[dbo].[Cards]') and col_name(fc.parent_object_id, fc.parent_column_id) = 'UsoCFDICode') " +
                "set @sql = 'alter table [dbo].[Cards] drop constraint [' + @fkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @fkName varchar(500) " +
                "set @fkName = (select f.name from sys.foreign_keys as f " +
                "inner join sys.foreign_key_columns as fc on f.object_id = fc.constraint_object_id " +
                "where f.parent_object_id = object_id('[dbo].[ARInvoices]') and col_name(fc.parent_object_id, fc.parent_column_id) = 'UsoCFDICode') " +
                "set @sql = 'alter table [dbo].[ARInvoices] drop constraint [' + @fkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @fkName varchar(500) " +
                "set @fkName = (select f.name from sys.foreign_keys as f " +
                "inner join sys.foreign_key_columns as fc on f.object_id = fc.constraint_object_id " +
                "where f.parent_object_id = object_id('[dbo].[DocumentsFilings]') and col_name(fc.parent_object_id, fc.parent_column_id) = 'StatusCode') " +
                "set @sql = 'alter table [dbo].[DocumentsFilings] drop constraint [' + @fkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @fkName varchar(500) " +
                "set @fkName = (select f.name from sys.foreign_keys as f " +
                "inner join sys.foreign_key_columns as fc on f.object_id = fc.constraint_object_id " +
                "where f.parent_object_id = object_id('[Customs].[ClaimsRelatedEntities]') and col_name(fc.parent_object_id, fc.parent_column_id) = 'DecisionCode') " +
                "set @sql = 'alter table [Customs].[ClaimsRelatedEntities] drop constraint [' + @fkName + ']' " +
                "exec(@sql)");



            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[Cards]') and col_name(i.object_id, ic.column_id) = 'UsoCFDICode') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[Cards]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[Customers]') and col_name(i.object_id, ic.column_id) = 'CustomerSizeId') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[Customers]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[DocumentsFilings]') and col_name(i.object_id, ic.column_id) = 'StatusCode') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[DocumentsFilings]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[ARInvoices]') and col_name(i.object_id, ic.column_id) = 'UsoCFDICode') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[ARInvoices]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[Customs].[ClaimsRelatedEntities]') and col_name(i.object_id, ic.column_id) = 'DecisionCode') " +
                "set @sql = 'drop index [' + @indexName + '] on [Customs].[ClaimsRelatedEntities]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[Customs].[DocumentTypeCustomsData]') and col_name(i.object_id, ic.column_id) = 'CustomsDoucumentTypeCode') " +
                "set @sql = 'drop index [' + @indexName + '] on [Customs].[DocumentTypeCustomsData]' " +
                "exec(@sql)");



            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'CustomerSizes') " +
                "set @sql = 'alter table [dbo].[CustomerSizes] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'UsoCFDIs') " +
                "set @sql = 'alter table [dbo].[UsoCFDIs] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'DocumentStatus') " +
                "set @sql = 'alter table [dbo].[DocumentStatus] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'CarrierAreasPorts') " +
                "set @sql = 'alter table [dbo].[CarrierAreasPorts] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            //Sql("declare @sql nvarchar(2000) " +
            //    "declare @pkName varchar(500) " +
            //    "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'Customs' and table_name = 'DecisionTypes') " +
            //    "set @sql = 'alter table [Customs].[DecisionTypes] drop constraint [' + @pkName + ']' " +
            //    "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'Customs' and table_name = 'DocumentTypeCustomsData') " +
                "set @sql = 'alter table [Customs].[DocumentTypeCustomsData] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'DWObjectFieldCategories') " +
                "set @sql = 'alter table [dbo].[DWObjectFieldCategories] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");



            AlterColumn("dbo.Cards", "UsoCFDICode", c => c.String(maxLength: 3, unicode: false));
            AlterColumn("dbo.Customers", "CustomerSizeId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.CustomerSizes", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.CustomerSizes", "Name", c => c.String(maxLength: 60));
            AlterColumn("dbo.CustomerSizes", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("dbo.UsoCFDIs", "Code", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AlterColumn("dbo.UsoCFDIs", "Name", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AlterColumn("dbo.UsoCFDIs", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("dbo.DocumentsFilings", "StatusCode", c => c.String(maxLength: 4, fixedLength: true, unicode: false));
            AlterColumn("dbo.DocumentStatus", "Code", c => c.String(nullable: false, maxLength: 4, fixedLength: true, unicode: false));
            AlterColumn("dbo.DocumentStatus", "Name", c => c.String(nullable: false, maxLength: 40, unicode: false));
            AlterColumn("dbo.DocumentStatus", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("dbo.ARInvoices", "UsoCFDICode", c => c.String(maxLength: 3, unicode: false));
            AlterColumn("dbo.CarrierAreasPorts", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("Customs.ClaimsRelatedEntities", "DecisionCode", c => c.String(maxLength: 2, unicode: false));
            AlterColumn("Customs.DecisionTypes", "Code", c => c.String(nullable: false, maxLength: 2, unicode: false));
            AlterColumn("Customs.DecisionTypes", "LocalName", c => c.String(maxLength: 50));
            AlterColumn("Customs.DecisionTypes", "EnglishName", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("Customs.DecisionTypes", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("Customs.DocumentTypeCustomsData", "DocumentTypeId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("Customs.DocumentTypeCustomsData", "CustomsDoucumentTypeCode", c => c.String(nullable: false, maxLength: 7, unicode: false));
            AlterColumn("dbo.DWObjectFieldCategories", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AddPrimaryKey("dbo.CustomerSizes", "Id");
            AddPrimaryKey("dbo.UsoCFDIs", "Code");
            AddPrimaryKey("dbo.DocumentStatus", "Code");
            AddPrimaryKey("dbo.CarrierAreasPorts", "Id");
            //AddPrimaryKey("Customs.DecisionTypes", "Code");
            AddPrimaryKey("Customs.DocumentTypeCustomsData", "DocumentTypeId");
            AddPrimaryKey("dbo.DWObjectFieldCategories", "Id");
            CreateIndex("dbo.Cards", "UsoCFDICode");
            CreateIndex("dbo.Customers", "CustomerSizeId");
            CreateIndex("dbo.DocumentsFilings", "StatusCode");
            CreateIndex("dbo.ARInvoices", "UsoCFDICode");
            CreateIndex("Customs.ClaimsRelatedEntities", "DecisionCode");
            CreateIndex("Customs.DocumentTypeCustomsData", "CustomsDoucumentTypeCode");
            AddForeignKey("dbo.Customers", "CustomerSizeId", "dbo.CustomerSizes", "Id");
            AddForeignKey("dbo.Cards", "UsoCFDICode", "dbo.UsoCFDIs", "Code");
            AddForeignKey("dbo.ARInvoices", "UsoCFDICode", "dbo.UsoCFDIs", "Code");
            AddForeignKey("dbo.DocumentsFilings", "StatusCode", "dbo.DocumentStatus", "Code");
            AddForeignKey("Customs.ClaimsRelatedEntities", "DecisionCode", "Customs.DecisionTypes", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.ClaimsRelatedEntities", "DecisionCode", "Customs.DecisionTypes");
            DropForeignKey("dbo.DocumentsFilings", "StatusCode", "dbo.DocumentStatus");
            DropForeignKey("dbo.ARInvoices", "UsoCFDICode", "dbo.UsoCFDIs");
            DropForeignKey("dbo.Cards", "UsoCFDICode", "dbo.UsoCFDIs");
            DropForeignKey("dbo.Customers", "CustomerSizeId", "dbo.CustomerSizes");
            DropIndex("Customs.DocumentTypeCustomsData", new[] { "CustomsDoucumentTypeCode" });
            DropIndex("Customs.ClaimsRelatedEntities", new[] { "DecisionCode" });
            DropIndex("dbo.ARInvoices", new[] { "UsoCFDICode" });
            DropIndex("dbo.DocumentsFilings", new[] { "StatusCode" });
            DropIndex("dbo.Customers", new[] { "CustomerSizeId" });
            DropIndex("dbo.Cards", new[] { "UsoCFDICode" });
            DropPrimaryKey("dbo.DWObjectFieldCategories");
            DropPrimaryKey("Customs.DocumentTypeCustomsData");
            DropPrimaryKey("Customs.DecisionTypes");
            DropPrimaryKey("dbo.CarrierAreasPorts");
            DropPrimaryKey("dbo.DocumentStatus");
            DropPrimaryKey("dbo.UsoCFDIs");
            DropPrimaryKey("dbo.CustomerSizes");
            AlterColumn("dbo.DWObjectFieldCategories", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("Customs.DocumentTypeCustomsData", "CustomsDoucumentTypeCode", c => c.String(maxLength: 7, unicode: false));
            AlterColumn("Customs.DocumentTypeCustomsData", "DocumentTypeId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("Customs.DecisionTypes", "SearchFields", c => c.String());
            AlterColumn("Customs.DecisionTypes", "EnglishName", c => c.String());
            AlterColumn("Customs.DecisionTypes", "LocalName", c => c.String());
            AlterColumn("Customs.DecisionTypes", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("Customs.ClaimsRelatedEntities", "DecisionCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.CarrierAreasPorts", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ARInvoices", "UsoCFDICode", c => c.String(maxLength: 128));
            AlterColumn("dbo.DocumentStatus", "SearchFields", c => c.String());
            AlterColumn("dbo.DocumentStatus", "Name", c => c.String());
            AlterColumn("dbo.DocumentStatus", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.DocumentsFilings", "StatusCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.UsoCFDIs", "SearchFields", c => c.String());
            AlterColumn("dbo.UsoCFDIs", "Name", c => c.String());
            AlterColumn("dbo.UsoCFDIs", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.CustomerSizes", "SearchFields", c => c.String());
            AlterColumn("dbo.CustomerSizes", "Name", c => c.String());
            AlterColumn("dbo.CustomerSizes", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Customers", "CustomerSizeId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Cards", "UsoCFDICode", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.DWObjectFieldCategories", "Id");
            AddPrimaryKey("Customs.DocumentTypeCustomsData", "DocumentTypeId");
            AddPrimaryKey("Customs.DecisionTypes", "Code");
            AddPrimaryKey("dbo.CarrierAreasPorts", "Id");
            AddPrimaryKey("dbo.DocumentStatus", "Code");
            AddPrimaryKey("dbo.UsoCFDIs", "Code");
            AddPrimaryKey("dbo.CustomerSizes", "Id");
            CreateIndex("Customs.DocumentTypeCustomsData", "CustomsDoucumentTypeCode");
            CreateIndex("Customs.ClaimsRelatedEntities", "DecisionCode");
            CreateIndex("dbo.ARInvoices", "UsoCFDICode");
            CreateIndex("dbo.DocumentsFilings", "StatusCode");
            CreateIndex("dbo.Customers", "CustomerSizeId");
            CreateIndex("dbo.Cards", "UsoCFDICode");
            AddForeignKey("Customs.ClaimsRelatedEntities", "DecisionCode", "dbo.DecisionTypes", "Code");
            AddForeignKey("dbo.DocumentsFilings", "StatusCode", "dbo.DocumentStatus", "Code");
            AddForeignKey("dbo.ARInvoices", "UsoCFDICode", "dbo.UsoCFDIs", "Code");
            AddForeignKey("dbo.Cards", "UsoCFDICode", "dbo.UsoCFDIs", "Code");
            AddForeignKey("dbo.Customers", "CustomerSizeId", "dbo.CustomerSizes", "Id");
            MoveTable(name: "Customs.DocumentTypeCustomsData", newSchema: "dbo");
            MoveTable(name: "Customs.DecisionTypes", newSchema: "dbo");
            RenameTable(name: "dbo.DocumentTypeCustomsData", newName: "DocumentTypeCustomsDatas");
        }
    }
}
