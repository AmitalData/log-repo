namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Abed_AddMissingPocosAndMapsToLogitudeContext : DbMigration
    {
        public override void Up()
        {
            Sql("declare @sql nvarchar(2000) " +
                "declare @fkName varchar(500) " +
                "set @fkName = (select f.name from sys.foreign_keys as f " +
                "inner join sys.foreign_key_columns as fc on f.object_id = fc.constraint_object_id " +
                "where f.parent_object_id = object_id('[dbo].[Tenants]') and col_name(fc.parent_object_id, fc.parent_column_id) = 'NumberFormatCode') " +
                "set @sql = 'alter table [dbo].[Tenants] drop constraint [' + @fkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @fkName varchar(500) " +
                "set @fkName = (select f.name from sys.foreign_keys as f " +
                "inner join sys.foreign_key_columns as fc on f.object_id = fc.constraint_object_id " +
                "where f.parent_object_id = object_id('[dbo].[AccountingTransferLines]') and col_name(fc.parent_object_id, fc.parent_column_id) = 'AccountingTransferHeaderId') " +
                "set @sql = 'alter table [dbo].[AccountingTransferLines] drop constraint [' + @fkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @fkName varchar(500) " +
                "set @fkName = (select f.name from sys.foreign_keys as f " +
                "inner join sys.foreign_key_columns as fc on f.object_id = fc.constraint_object_id " +
                "where f.parent_object_id = object_id('[dbo].[AccountingTransferHeaders]') and col_name(fc.parent_object_id, fc.parent_column_id) = 'AccountingTransferTypeCode') " +
                "set @sql = 'alter table [dbo].[AccountingTransferHeaders] drop constraint [' + @fkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @fkName varchar(500) " +
                "set @fkName = (select f.name from sys.foreign_keys as f " +
                "inner join sys.foreign_key_columns as fc on f.object_id = fc.constraint_object_id " +
                "where f.parent_object_id = object_id('[dbo].[AWBOCIs]') and col_name(fc.parent_object_id, fc.parent_column_id) = 'AWBCustomsInformationCode') " +
                "set @sql = 'alter table [dbo].[AWBOCIs] drop constraint [' + @fkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @fkName varchar(500) " +
                "set @fkName = (select f.name from sys.foreign_keys as f " +
                "inner join sys.foreign_key_columns as fc on f.object_id = fc.constraint_object_id " +
                "where f.parent_object_id = object_id('[dbo].[AWBOCIs]') and col_name(fc.parent_object_id, fc.parent_column_id) = 'AWBInformationCode') " +
                "set @sql = 'alter table [dbo].[AWBOCIs] drop constraint [' + @fkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @fkName varchar(500) " +
                "set @fkName = (select f.name from sys.foreign_keys as f " +
                "inner join sys.foreign_key_columns as fc on f.object_id = fc.constraint_object_id " +
                "where f.parent_object_id = object_id('[dbo].[EventTypes]') and col_name(fc.parent_object_id, fc.parent_column_id) = 'EventTypeCategoryCode') " +
                "set @sql = 'alter table [dbo].[EventTypes] drop constraint [' + @fkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @fkName varchar(500) " +
                "set @fkName = (select f.name from sys.foreign_keys as f " +
                "inner join sys.foreign_key_columns as fc on f.object_id = fc.constraint_object_id " +
                "where f.parent_object_id = object_id('[dbo].[Reports]') and col_name(fc.parent_object_id, fc.parent_column_id) = 'ReportGroupId') " +
                "set @sql = 'alter table [dbo].[Reports] drop constraint [' + @fkName + ']' " +
                "exec(@sql)");



            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[Tenants]') and col_name(i.object_id, ic.column_id) = 'PasswordPolicyCode') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[Tenants]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[Tenants]') and col_name(i.object_id, ic.column_id) = 'NumberFormatCode') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[Tenants]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[AccountingTransferHeaders]') and col_name(i.object_id, ic.column_id) = 'AccountingTransferTypeCode') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[AccountingTransferHeaders]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[AccountingTransferHeaders]') and col_name(i.object_id, ic.column_id) = 'UserId') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[AccountingTransferHeaders]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[AccountingTransferLines]') and col_name(i.object_id, ic.column_id) = 'AccountingTransferHeaderId') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[AccountingTransferLines]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[AWBOCIs]') and col_name(i.object_id, ic.column_id) = 'ShipmentId') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[AWBOCIs]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[AWBOCIs]') and col_name(i.object_id, ic.column_id) = 'AWBCustomsInformationCode') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[AWBOCIs]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[AWBOCIs]') and col_name(i.object_id, ic.column_id) = 'AWBInformationCode') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[AWBOCIs]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[EventTypes]') and col_name(i.object_id, ic.column_id) = 'EventTypeCategoryCode') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[EventTypes]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[ChargeTypeAccountings]') and col_name(i.object_id, ic.column_id) = 'VatTypeId') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[ChargeTypeAccountings]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[ChargeTypeAccountings]') and col_name(i.object_id, ic.column_id) = 'ChargeTypeId') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[ChargeTypeAccountings]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[ContactLoginLogs]') and col_name(i.object_id, ic.column_id) = 'ContactId') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[ContactLoginLogs]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[DWSubQueries]') and col_name(i.object_id, ic.column_id) = 'DWQueryId') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[DWSubQueries]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[QueueMessageMoreDetails]') and col_name(i.object_id, ic.column_id) = 'QueueDefinitionCode') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[QueueMessageMoreDetails]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[Reports]') and col_name(i.object_id, ic.column_id) = 'ReportGroupId') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[Reports]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[UserPermittedBranches]') and col_name(i.object_id, ic.column_id) = 'UserId') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[UserPermittedBranches]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[UserPermittedBranches]') and col_name(i.object_id, ic.column_id) = 'BranchId') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[UserPermittedBranches]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[UserPermittedProducts]') and col_name(i.object_id, ic.column_id) = 'UserId') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[UserPermittedProducts]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[UserPermittedProducts]') and col_name(i.object_id, ic.column_id) = 'ProductTypeCode') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[UserPermittedProducts]' " +
                "exec(@sql)");



            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'NumberFormats') " +
                "set @sql = 'alter table [dbo].[NumberFormats] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'AccountingTransferHeaders') " +
                "set @sql = 'alter table [dbo].[AccountingTransferHeaders] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'AccountingTransferTypes') " +
                "set @sql = 'alter table [dbo].[AccountingTransferTypes] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'AccountingTransferLines') " +
                "set @sql = 'alter table [dbo].[AccountingTransferLines] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'AWBCustomsInformations') " +
                "set @sql = 'alter table [dbo].[AWBCustomsInformations] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'AWBInformations') " +
                "set @sql = 'alter table [dbo].[AWBInformations] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'AWBOCIs') " +
                "set @sql = 'alter table [dbo].[AWBOCIs] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'EventTypeCategories') " +
                "set @sql = 'alter table [dbo].[EventTypeCategories] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'ChargeTypeAccountings') " +
                "set @sql = 'alter table [dbo].[ChargeTypeAccountings] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'ContactLoginLogs') " +
                "set @sql = 'alter table [dbo].[ContactLoginLogs] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'DWSubQueries') " +
                "set @sql = 'alter table [dbo].[DWSubQueries] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'GeneralLocks') " +
                "set @sql = 'alter table [dbo].[GeneralLocks] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'HybridTenantStates') " +
                "set @sql = 'alter table [dbo].[HybridTenantStates] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'HybridTenantThresholds') " +
                "set @sql = 'alter table [dbo].[HybridTenantThresholds] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'ReportGroups') " +
                "set @sql = 'alter table [dbo].[ReportGroups] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'UserPermittedBranches') " +
                "set @sql = 'alter table [dbo].[UserPermittedBranches] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'UserPermittedProducts') " +
                "set @sql = 'alter table [dbo].[UserPermittedProducts] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");



            //AddColumn("dbo.Cards", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Users", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Branches", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Addresses", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Countries", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.States", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Contacts", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Departments", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Customers", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Ranks", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Currencies", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Tenants", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.PartnerTypes", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Directions", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Ports", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Incoterms", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.TransportModes", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.ShipmentTypes", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.ShipmentLevels", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Shipments", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.Shipments", "SLAC", c => c.String(maxLength: 5, unicode: false));
            //AddColumn("dbo.EntityStatus", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.ShipmentMasterDatas", "AutomaticLastUpdateDate", c => c.DateTime());
            //AddColumn("dbo.TasksScheduler", "EntityId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Contacts", "ExternalId", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Tenants", "PasswordPolicyCode", c => c.String(maxLength: 4, unicode: false));
            AlterColumn("dbo.Tenants", "NumberFormatCode", c => c.String(maxLength: 4, unicode: false));
            AlterColumn("dbo.NumberFormats", "Code", c => c.String(nullable: false, maxLength: 4, unicode: false));
            AlterColumn("dbo.NumberFormats", "Name", c => c.String(nullable: false, maxLength: 40, unicode: false));
            AlterColumn("dbo.NumberFormats", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("dbo.AccountingTransferHeaders", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));//
            AlterColumn("dbo.AccountingTransferHeaders", "TransferNumber", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.AccountingTransferHeaders", "FileName", c => c.String(nullable: false, maxLength: 40, unicode: false));
            AlterColumn("dbo.AccountingTransferHeaders", "AccountingTransferTypeCode", c => c.String(nullable: false, maxLength: 4, unicode: false));
            AlterColumn("dbo.AccountingTransferHeaders", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("dbo.AccountingTransferHeaders", "UserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.AccountingTransferHeaders", "Notes", c => c.String(maxLength: 250));
            AlterColumn("dbo.AccountingTransferTypes", "Code", c => c.String(nullable: false, maxLength: 4, unicode: false));//
            AlterColumn("dbo.AccountingTransferTypes", "Name", c => c.String(nullable: false, maxLength: 40, unicode: false));
            AlterColumn("dbo.AccountingTransferTypes", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("dbo.AccountingTransferLines", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.AccountingTransferLines", "EntityId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.AccountingTransferLines", "AccountingTransferHeaderId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.AccountingTransferLines", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("dbo.AccountingTransferLines", "EntityReference", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.ObjectTables", "CodeField", c => c.String(maxLength: 60, unicode: false));
            AlterColumn("dbo.ObjectTables", "SplitComponentPath", c => c.String());
            AlterColumn("dbo.Features", "Code", c => c.String(nullable: false, maxLength: 120, unicode: false));
            AlterColumn("dbo.AWBCustomsInformations", "Code", c => c.String(nullable: false, maxLength: 2, unicode: false));
            AlterColumn("dbo.AWBCustomsInformations", "Name", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AlterColumn("dbo.AWBCustomsInformations", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("dbo.AWBInformations", "Code", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AlterColumn("dbo.AWBInformations", "Name", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AlterColumn("dbo.AWBInformations", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("dbo.AWBOCIs", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.AWBOCIs", "ShipmentId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.AWBOCIs", "AWBCustomsInformationCode", c => c.String(maxLength: 2, unicode: false));
            AlterColumn("dbo.AWBOCIs", "AWBInformationCode", c => c.String(maxLength: 3, unicode: false));
            AlterColumn("dbo.AWBOCIs", "SupplementaryCustomsInfo", c => c.String(nullable: false, maxLength: 35, unicode: false));
            AlterColumn("dbo.Shipments", "AccountedReceivablesInLocalCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.EventTypes", "EventTypeCategoryCode", c => c.String(maxLength: 4));
            AlterColumn("dbo.EventTypeCategories", "Code", c => c.String(nullable: false, maxLength: 4));
            AlterColumn("dbo.EventTypeCategories", "Name", c => c.String(nullable: false, maxLength: 40, unicode: false));
            AlterColumn("dbo.EventTypeCategories", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("dbo.ShipmentAdditionalCloudDatas", "ApprovedByUserName", c => c.String(maxLength: 200));
            AlterColumn("dbo.ShipmentAdditionalCloudDatas", "VersionApproved", c => c.String(maxLength: 10));
            AlterColumn("dbo.ShipmentAdditionalCloudDatas", "DenyReason", c => c.String(maxLength: 1024));
            AlterColumn("dbo.ShipmentAdditionalCloudDatas", "ShipmentAddtionalDataXML", c => c.String(maxLength: 4000));
            AlterColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumber", c => c.String(maxLength: 35));
            AlterColumn("dbo.ChargeTypeAccountings", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.ChargeTypeAccountings", "PayableDebitAccount", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.ChargeTypeAccountings", "PayableDebitGLAcountId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.ChargeTypeAccountings", "ReceivableCreditAccount", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.ChargeTypeAccountings", "ReceivableCreditGLAccountId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.ChargeTypeAccountings", "VatTypeId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.ChargeTypeAccountings", "ChargeTypeId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.ContactLoginLogs", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.ContactLoginLogs", "IP", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.ContactLoginLogs", "Browser", c => c.String(maxLength: 40, unicode: false));
            AlterColumn("dbo.ContactLoginLogs", "ContactId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.ContactLoginLogs", "ComputerId", c => c.String(nullable: false, maxLength: 40, unicode: false));
            AlterColumn("dbo.ContactLoginLogs", "ContactAgent", c => c.String(maxLength: 400));
            AlterColumn("dbo.ContactLoginLogs", "Via", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.DWSubQueries", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.DWSubQueries", "DWFactTableCode", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.DWSubQueries", "DWQueryId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.Followers", "CancelledDate", c => c.DateTime());
            AlterColumn("dbo.GeneralLocks", "GeneralKey", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AlterColumn("dbo.HybridTenantStates", "Tenant", c => c.Int(nullable: false));
            AlterColumn("dbo.HybridTenantThresholds", "Tenant", c => c.Int(nullable: false));
            AlterColumn("dbo.QueueMessageMoreDetails", "QueueDefinitionCode", c => c.String(nullable: false, maxLength: 265, unicode: false));
            AlterColumn("dbo.QueueMessageMoreDetails", "MessageBody", c => c.String(nullable: false, maxLength: 1000, unicode: false));
            AlterColumn("dbo.QueueMessageMoreDetails", "Field1", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.QueueMessageMoreDetails", "Field2", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.QueueMessageMoreDetails", "Field3", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.ReportGroups", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.ReportGroups", "Code", c => c.String(nullable: false, maxLength: 4, unicode: false));
            AlterColumn("dbo.ReportGroups", "EnglishName", c => c.String(nullable: false, maxLength: 40, unicode: false));
            AlterColumn("dbo.ReportGroups", "LocalName", c => c.String(maxLength: 40));
            AlterColumn("dbo.Reports", "ReportGroupId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.TenantAdditionalDatas", "DropBoxAccessToken", c => c.String(maxLength: 350, unicode: false));
            AlterColumn("dbo.TenantAdditionalDatas", "DropBoxState", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.TenantAdditionalDatas", "DropBoxUID", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.TenantAdditionalDatas", "DropBoxUEmail", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.TenantAdditionalDatas", "PaymentGatewayConnectionString", c => c.String(maxLength: 400, unicode: false));
            AlterColumn("dbo.UserPermittedBranches", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.UserPermittedBranches", "UserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.UserPermittedBranches", "BranchId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.UserPermittedProducts", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.UserPermittedProducts", "UserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.UserPermittedProducts", "ProductTypeCode", c => c.String(nullable: false, maxLength: 2, unicode: false));
            AddPrimaryKey("dbo.NumberFormats", "Code");
            AddPrimaryKey("dbo.AccountingTransferHeaders", "Id");
            AddPrimaryKey("dbo.AccountingTransferTypes", "Code");
            AddPrimaryKey("dbo.AccountingTransferLines", "Id");
            AddPrimaryKey("dbo.AWBCustomsInformations", "Code");
            AddPrimaryKey("dbo.AWBInformations", "Code");
            AddPrimaryKey("dbo.AWBOCIs", "Id");
            AddPrimaryKey("dbo.EventTypeCategories", "Code");
            AddPrimaryKey("dbo.ChargeTypeAccountings", "Id");
            AddPrimaryKey("dbo.ContactLoginLogs", "Id");
            AddPrimaryKey("dbo.DWSubQueries", "Id");
            AddPrimaryKey("dbo.GeneralLocks", new[] { "GeneralKey", "Tenant" });
            AddPrimaryKey("dbo.HybridTenantStates", "Tenant");
            AddPrimaryKey("dbo.HybridTenantThresholds", "Tenant");
            AddPrimaryKey("dbo.ReportGroups", "Id");
            AddPrimaryKey("dbo.UserPermittedBranches", "Id");
            AddPrimaryKey("dbo.UserPermittedProducts", "Id");
            CreateIndex("dbo.Tenants", "PasswordPolicyCode");
            CreateIndex("dbo.Tenants", "NumberFormatCode");
            CreateIndex("dbo.AccountingTransferHeaders", "AccountingTransferTypeCode");
            CreateIndex("dbo.AccountingTransferHeaders", "UserId");
            CreateIndex("dbo.AccountingTransferLines", "AccountingTransferHeaderId");
            CreateIndex("dbo.AWBOCIs", "ShipmentId");
            CreateIndex("dbo.AWBOCIs", "AWBCustomsInformationCode");
            CreateIndex("dbo.AWBOCIs", "AWBInformationCode");
            CreateIndex("dbo.EventTypes", "EventTypeCategoryCode");
            CreateIndex("dbo.ChargeTypeAccountings", "VatTypeId");
            CreateIndex("dbo.ChargeTypeAccountings", "ChargeTypeId");
            CreateIndex("dbo.ContactLoginLogs", "ContactId");
            CreateIndex("dbo.DWSubQueries", "DWQueryId");
            CreateIndex("dbo.QueueMessageMoreDetails", "QueueDefinitionCode");
            CreateIndex("dbo.Reports", "ReportGroupId");
            CreateIndex("dbo.UserPermittedBranches", "UserId");
            CreateIndex("dbo.UserPermittedBranches", "BranchId");
            CreateIndex("dbo.UserPermittedProducts", "UserId");
            CreateIndex("dbo.UserPermittedProducts", "ProductTypeCode");
            AddForeignKey("dbo.Tenants", "NumberFormatCode", "dbo.NumberFormats", "Code");
            AddForeignKey("dbo.AccountingTransferLines", "AccountingTransferHeaderId", "dbo.AccountingTransferHeaders", "Id");
            AddForeignKey("dbo.AccountingTransferHeaders", "AccountingTransferTypeCode", "dbo.AccountingTransferTypes", "Code");
            AddForeignKey("dbo.AWBOCIs", "AWBCustomsInformationCode", "dbo.AWBCustomsInformations", "Code");
            AddForeignKey("dbo.AWBOCIs", "AWBInformationCode", "dbo.AWBInformations", "Code");
            AddForeignKey("dbo.EventTypes", "EventTypeCategoryCode", "dbo.EventTypeCategories", "Code");
            AddForeignKey("dbo.Reports", "ReportGroupId", "dbo.ReportGroups", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Reports", "ReportGroupId", "dbo.ReportGroups");
            DropForeignKey("dbo.EventTypes", "EventTypeCategoryCode", "dbo.EventTypeCategories");
            DropForeignKey("dbo.AWBOCIs", "AWBInformationCode", "dbo.AWBInformations");
            DropForeignKey("dbo.AWBOCIs", "AWBCustomsInformationCode", "dbo.AWBCustomsInformations");
            DropForeignKey("dbo.AccountingTransferHeaders", "AccountingTransferTypeCode", "dbo.AccountingTransferTypes");
            DropForeignKey("dbo.AccountingTransferLines", "AccountingTransferHeaderId", "dbo.AccountingTransferHeaders");
            DropForeignKey("dbo.Tenants", "NumberFormatCode", "dbo.NumberFormats");
            DropIndex("dbo.UserPermittedProducts", new[] { "ProductTypeCode" });
            DropIndex("dbo.UserPermittedProducts", new[] { "UserId" });
            DropIndex("dbo.UserPermittedBranches", new[] { "BranchId" });
            DropIndex("dbo.UserPermittedBranches", new[] { "UserId" });
            DropIndex("dbo.Reports", new[] { "ReportGroupId" });
            DropIndex("dbo.QueueMessageMoreDetails", new[] { "QueueDefinitionCode" });
            DropIndex("dbo.DWSubQueries", new[] { "DWQueryId" });
            DropIndex("dbo.ContactLoginLogs", new[] { "ContactId" });
            DropIndex("dbo.ChargeTypeAccountings", new[] { "ChargeTypeId" });
            DropIndex("dbo.ChargeTypeAccountings", new[] { "VatTypeId" });
            DropIndex("dbo.EventTypes", new[] { "EventTypeCategoryCode" });
            DropIndex("dbo.AWBOCIs", new[] { "AWBInformationCode" });
            DropIndex("dbo.AWBOCIs", new[] { "AWBCustomsInformationCode" });
            DropIndex("dbo.AWBOCIs", new[] { "ShipmentId" });
            DropIndex("dbo.AccountingTransferLines", new[] { "AccountingTransferHeaderId" });
            DropIndex("dbo.AccountingTransferHeaders", new[] { "UserId" });
            DropIndex("dbo.AccountingTransferHeaders", new[] { "AccountingTransferTypeCode" });
            DropIndex("dbo.Tenants", new[] { "NumberFormatCode" });
            DropIndex("dbo.Tenants", new[] { "PasswordPolicyCode" });
            DropPrimaryKey("dbo.UserPermittedProducts");
            DropPrimaryKey("dbo.UserPermittedBranches");
            DropPrimaryKey("dbo.ReportGroups");
            DropPrimaryKey("dbo.HybridTenantThresholds");
            DropPrimaryKey("dbo.HybridTenantStates");
            DropPrimaryKey("dbo.GeneralLocks");
            DropPrimaryKey("dbo.DWSubQueries");
            DropPrimaryKey("dbo.ContactLoginLogs");
            DropPrimaryKey("dbo.ChargeTypeAccountings");
            DropPrimaryKey("dbo.EventTypeCategories");
            DropPrimaryKey("dbo.AWBOCIs");
            DropPrimaryKey("dbo.AWBInformations");
            DropPrimaryKey("dbo.AWBCustomsInformations");
            DropPrimaryKey("dbo.AccountingTransferLines");
            DropPrimaryKey("dbo.AccountingTransferTypes");
            DropPrimaryKey("dbo.AccountingTransferHeaders");
            DropPrimaryKey("dbo.NumberFormats");
            AlterColumn("dbo.UserPermittedProducts", "ProductTypeCode", c => c.String(maxLength: 2, unicode: false));
            AlterColumn("dbo.UserPermittedProducts", "UserId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.UserPermittedProducts", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.UserPermittedBranches", "BranchId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.UserPermittedBranches", "UserId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.UserPermittedBranches", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.TenantAdditionalDatas", "PaymentGatewayConnectionString", c => c.String());
            AlterColumn("dbo.TenantAdditionalDatas", "DropBoxUEmail", c => c.String());
            AlterColumn("dbo.TenantAdditionalDatas", "DropBoxUID", c => c.String());
            AlterColumn("dbo.TenantAdditionalDatas", "DropBoxState", c => c.String());
            AlterColumn("dbo.TenantAdditionalDatas", "DropBoxAccessToken", c => c.String());
            AlterColumn("dbo.Reports", "ReportGroupId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ReportGroups", "LocalName", c => c.String());
            AlterColumn("dbo.ReportGroups", "EnglishName", c => c.String());
            AlterColumn("dbo.ReportGroups", "Code", c => c.String());
            AlterColumn("dbo.ReportGroups", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.QueueMessageMoreDetails", "Field3", c => c.String());
            AlterColumn("dbo.QueueMessageMoreDetails", "Field2", c => c.String());
            AlterColumn("dbo.QueueMessageMoreDetails", "Field1", c => c.String());
            AlterColumn("dbo.QueueMessageMoreDetails", "MessageBody", c => c.String());
            AlterColumn("dbo.QueueMessageMoreDetails", "QueueDefinitionCode", c => c.String(maxLength: 265, unicode: false));
            AlterColumn("dbo.HybridTenantThresholds", "Tenant", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.HybridTenantStates", "Tenant", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.GeneralLocks", "GeneralKey", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Followers", "CancelledDate", c => c.Boolean());
            AlterColumn("dbo.DWSubQueries", "DWQueryId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.DWSubQueries", "DWFactTableCode", c => c.String());
            AlterColumn("dbo.DWSubQueries", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ContactLoginLogs", "Via", c => c.String());
            AlterColumn("dbo.ContactLoginLogs", "ContactAgent", c => c.String());
            AlterColumn("dbo.ContactLoginLogs", "ComputerId", c => c.String());
            AlterColumn("dbo.ContactLoginLogs", "ContactId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.ContactLoginLogs", "Browser", c => c.String());
            AlterColumn("dbo.ContactLoginLogs", "IP", c => c.String());
            AlterColumn("dbo.ContactLoginLogs", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ChargeTypeAccountings", "ChargeTypeId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.ChargeTypeAccountings", "VatTypeId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.ChargeTypeAccountings", "ReceivableCreditGLAccountId", c => c.String());
            AlterColumn("dbo.ChargeTypeAccountings", "ReceivableCreditAccount", c => c.String());
            AlterColumn("dbo.ChargeTypeAccountings", "PayableDebitGLAcountId", c => c.String());
            AlterColumn("dbo.ChargeTypeAccountings", "PayableDebitAccount", c => c.String());
            AlterColumn("dbo.ChargeTypeAccountings", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumber", c => c.String());
            AlterColumn("dbo.ShipmentAdditionalCloudDatas", "ShipmentAddtionalDataXML", c => c.String());
            AlterColumn("dbo.ShipmentAdditionalCloudDatas", "DenyReason", c => c.String());
            AlterColumn("dbo.ShipmentAdditionalCloudDatas", "VersionApproved", c => c.String());
            AlterColumn("dbo.ShipmentAdditionalCloudDatas", "ApprovedByUserName", c => c.String());
            AlterColumn("dbo.EventTypeCategories", "SearchFields", c => c.String());
            AlterColumn("dbo.EventTypeCategories", "Name", c => c.String());
            AlterColumn("dbo.EventTypeCategories", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.EventTypes", "EventTypeCategoryCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.Shipments", "AccountedReceivablesInLocalCurrency", c => c.Double());
            AlterColumn("dbo.AWBOCIs", "SupplementaryCustomsInfo", c => c.String());
            AlterColumn("dbo.AWBOCIs", "AWBInformationCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.AWBOCIs", "AWBCustomsInformationCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.AWBOCIs", "ShipmentId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.AWBOCIs", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AWBInformations", "SearchFields", c => c.String());
            AlterColumn("dbo.AWBInformations", "Name", c => c.String());
            AlterColumn("dbo.AWBInformations", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AWBCustomsInformations", "SearchFields", c => c.String());
            AlterColumn("dbo.AWBCustomsInformations", "Name", c => c.String());
            AlterColumn("dbo.AWBCustomsInformations", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Features", "Code", c => c.String(nullable: false, maxLength: 80, unicode: false));
            AlterColumn("dbo.ObjectTables", "SplitComponentPath", c => c.String(maxLength: 250));
            AlterColumn("dbo.ObjectTables", "CodeField", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.AccountingTransferLines", "EntityReference", c => c.String());
            AlterColumn("dbo.AccountingTransferLines", "SearchFields", c => c.String());
            AlterColumn("dbo.AccountingTransferLines", "AccountingTransferHeaderId", c => c.String(maxLength: 128));
            AlterColumn("dbo.AccountingTransferLines", "EntityId", c => c.String());
            AlterColumn("dbo.AccountingTransferLines", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AccountingTransferTypes", "SearchFields", c => c.String());
            AlterColumn("dbo.AccountingTransferTypes", "Name", c => c.String());
            AlterColumn("dbo.AccountingTransferTypes", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AccountingTransferHeaders", "Notes", c => c.String());
            AlterColumn("dbo.AccountingTransferHeaders", "UserId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.AccountingTransferHeaders", "SearchFields", c => c.String());
            AlterColumn("dbo.AccountingTransferHeaders", "AccountingTransferTypeCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.AccountingTransferHeaders", "FileName", c => c.String());
            AlterColumn("dbo.AccountingTransferHeaders", "TransferNumber", c => c.String());
            AlterColumn("dbo.AccountingTransferHeaders", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.NumberFormats", "SearchFields", c => c.String());
            AlterColumn("dbo.NumberFormats", "Name", c => c.String());
            AlterColumn("dbo.NumberFormats", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Tenants", "NumberFormatCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.Tenants", "PasswordPolicyCode", c => c.String(nullable: false, maxLength: 4, unicode: false));
            AlterColumn("dbo.Contacts", "ExternalId", c => c.String());
            //DropColumn("dbo.TasksScheduler", "EntityId");
            //DropColumn("dbo.ShipmentMasterDatas", "AutomaticLastUpdateDate");
            //DropColumn("dbo.EntityStatus", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Shipments", "SLAC");
            //DropColumn("dbo.Shipments", "AutomaticLastUpdateDate");
            //DropColumn("dbo.ShipmentLevels", "AutomaticLastUpdateDate");
            //DropColumn("dbo.ShipmentTypes", "AutomaticLastUpdateDate");
            //DropColumn("dbo.TransportModes", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Incoterms", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Ports", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Directions", "AutomaticLastUpdateDate");
            //DropColumn("dbo.PartnerTypes", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Tenants", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Currencies", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Ranks", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Customers", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Departments", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Contacts", "AutomaticLastUpdateDate");
            //DropColumn("dbo.States", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Countries", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Addresses", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Branches", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Users", "AutomaticLastUpdateDate");
            //DropColumn("dbo.Cards", "AutomaticLastUpdateDate");
            AddPrimaryKey("dbo.UserPermittedProducts", "Id");
            AddPrimaryKey("dbo.UserPermittedBranches", "Id");
            AddPrimaryKey("dbo.ReportGroups", "Id");
            AddPrimaryKey("dbo.HybridTenantThresholds", "Tenant");
            AddPrimaryKey("dbo.HybridTenantStates", "Tenant");
            AddPrimaryKey("dbo.GeneralLocks", new[] { "GeneralKey", "Tenant" });
            AddPrimaryKey("dbo.DWSubQueries", "Id");
            AddPrimaryKey("dbo.ContactLoginLogs", "Id");
            AddPrimaryKey("dbo.ChargeTypeAccountings", "Id");
            AddPrimaryKey("dbo.EventTypeCategories", "Code");
            AddPrimaryKey("dbo.AWBOCIs", "Id");
            AddPrimaryKey("dbo.AWBInformations", "Code");
            AddPrimaryKey("dbo.AWBCustomsInformations", "Code");
            AddPrimaryKey("dbo.AccountingTransferLines", "Id");
            AddPrimaryKey("dbo.AccountingTransferTypes", "Code");
            AddPrimaryKey("dbo.AccountingTransferHeaders", "Id");
            AddPrimaryKey("dbo.NumberFormats", "Code");
            CreateIndex("dbo.UserPermittedProducts", "ProductTypeCode");
            CreateIndex("dbo.UserPermittedProducts", "UserId");
            CreateIndex("dbo.UserPermittedBranches", "BranchId");
            CreateIndex("dbo.UserPermittedBranches", "UserId");
            CreateIndex("dbo.Reports", "ReportGroupId");
            CreateIndex("dbo.QueueMessageMoreDetails", "QueueDefinitionCode");
            CreateIndex("dbo.DWSubQueries", "DWQueryId");
            CreateIndex("dbo.ContactLoginLogs", "ContactId");
            CreateIndex("dbo.ChargeTypeAccountings", "ChargeTypeId");
            CreateIndex("dbo.ChargeTypeAccountings", "VatTypeId");
            CreateIndex("dbo.EventTypes", "EventTypeCategoryCode");
            CreateIndex("dbo.AWBOCIs", "AWBInformationCode");
            CreateIndex("dbo.AWBOCIs", "AWBCustomsInformationCode");
            CreateIndex("dbo.AWBOCIs", "ShipmentId");
            CreateIndex("dbo.AccountingTransferLines", "AccountingTransferHeaderId");
            CreateIndex("dbo.AccountingTransferHeaders", "UserId");
            CreateIndex("dbo.AccountingTransferHeaders", "AccountingTransferTypeCode");
            CreateIndex("dbo.Tenants", "NumberFormatCode");
            CreateIndex("dbo.Tenants", "PasswordPolicyCode");
            AddForeignKey("dbo.Reports", "ReportGroupId", "dbo.ReportGroups", "Id");
            AddForeignKey("dbo.EventTypes", "EventTypeCategoryCode", "dbo.EventTypeCategories", "Code");
            AddForeignKey("dbo.AWBOCIs", "AWBInformationCode", "dbo.AWBInformations", "Code");
            AddForeignKey("dbo.AWBOCIs", "AWBCustomsInformationCode", "dbo.AWBCustomsInformations", "Code");
            AddForeignKey("dbo.AccountingTransferHeaders", "AccountingTransferTypeCode", "dbo.AccountingTransferTypes", "Code");
            AddForeignKey("dbo.AccountingTransferLines", "AccountingTransferHeaderId", "dbo.AccountingTransferHeaders", "Id");
            AddForeignKey("dbo.Tenants", "NumberFormatCode", "dbo.NumberFormats", "Code");
        }
    }
}
