namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Abed_AddCourierCustomStatusToLogitudeContext_PocoAndMap : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.CourierCustomStatus", newName: "CourierCustomStatuses");
            //MoveTable(name: "dbo.CourierCustomStatuses", newSchema: "Customs");


            Sql("declare @sql nvarchar(2000) " +
                "declare @fkName varchar(500) " +
                "set @fkName = (select f.name from sys.foreign_keys as f " +
                "inner join sys.foreign_key_columns as fc on f.object_id = fc.constraint_object_id " +
                "where f.parent_object_id = object_id('[Customs].[Declarations]') and col_name(fc.parent_object_id, fc.parent_column_id) = 'CourierCustomStatusCode') " +
                "set @sql = 'alter table [Customs].[Declarations] drop constraint [' + @fkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[Customs].[Declarations]') and col_name(i.object_id, ic.column_id) = 'CourierCustomStatusCode') " +
                "set @sql = 'drop index [' + @indexName + '] on [Customs].[Declarations]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'Customs' and table_name = 'CourierCustomStatuses') " +
                "set @sql = 'alter table [Customs].[CourierCustomStatuses] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");


            //AddColumn("dbo.Users", "LayoutDirection", c => c.String(maxLength: 3, unicode: false));
            //AddColumn("dbo.TariffSettings", "ContainerDefaults", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("Customs.Declarations", "CourierCustomStatusCode", c => c.String(maxLength: 3, unicode: false));
            AlterColumn("Customs.CourierCustomStatuses", "Code", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AlterColumn("Customs.CourierCustomStatuses", "Name", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("Customs.CourierCustomStatuses", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("Customs.CourierCustomStatuses", "LocalName", c => c.String(maxLength: 100));
            AddPrimaryKey("Customs.CourierCustomStatuses", "Code");
            CreateIndex("Customs.Declarations", "CourierCustomStatusCode");
            AddForeignKey("Customs.Declarations", "CourierCustomStatusCode", "Customs.CourierCustomStatuses", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.Declarations", "FK_Customs.Declarations_Customs.CourierCustomStatus_CourierCustomStatusCode");
            DropIndex("Customs.Declarations", "IX_CourierCustomStatusCode");
            DropPrimaryKey("Customs.CourierCustomStatuses", "PK_Customs.CourierCustomStatus");
            AlterColumn("Customs.CourierCustomStatuses", "LocalName", c => c.String());
            AlterColumn("Customs.CourierCustomStatuses", "SearchFields", c => c.String());
            AlterColumn("Customs.CourierCustomStatuses", "Name", c => c.String());
            AlterColumn("Customs.CourierCustomStatuses", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("Customs.Declarations", "CourierCustomStatusCode", c => c.String(maxLength: 128));
            //DropColumn("dbo.TariffSettings", "ContainerDefaults");
            //DropColumn("dbo.Users", "LayoutDirection");
            AddPrimaryKey("Customs.CourierCustomStatuses", "Code");
            CreateIndex("Customs.Declarations", "CourierCustomStatusCode");
            AddForeignKey("Customs.Declarations", "CourierCustomStatusCode", "dbo.CourierCustomStatus", "Code");
            //MoveTable(name: "Customs.CourierCustomStatuses", newSchema: "dbo");
            //RenameTable(name: "dbo.CourierCustomStatuses", newName: "CourierCustomStatus");
        }
    }
}
