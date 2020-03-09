namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Abed_AddPaymentGatewayPartnerToLogitudeContext_PocoAndMap : DbMigration
    {
        public override void Up()
        {
            Sql("declare @sql nvarchar(2000) " +
                "declare @fkName varchar(500) " +
                "set @fkName = (select f.name from sys.foreign_keys as f " +
                "inner join sys.foreign_key_columns as fc on f.object_id = fc.constraint_object_id " +
                "where f.parent_object_id = object_id('[dbo].[TenantAdditionalDatas]') and col_name(fc.parent_object_id, fc.parent_column_id) = 'PaymentGatewayPartnerCode') " +
                "set @sql = 'alter table [dbo].[TenantAdditionalDatas] drop constraint [' + @fkName + ']' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @indexName varchar(500) " +
                "set @indexName = (select i.name from sys.indexes as i " +
                "inner join sys.index_columns as ic on i.object_id = ic.object_id AND i.index_id = ic.index_id " +
                "where i.is_unique = 0 and i.object_id = object_id('[dbo].[TenantAdditionalDatas]') and col_name(i.object_id, ic.column_id) = 'PaymentGatewayPartnerCode') " +
                "set @sql = 'drop index [' + @indexName + '] on [dbo].[TenantAdditionalDatas]' " +
                "exec(@sql)");

            Sql("declare @sql nvarchar(2000) " +
                "declare @pkName varchar(500) " +
                "set @pkName = (select constraint_name from information_schema.table_constraints where constraint_type = 'PRIMARY KEY' and table_schema = 'dbo' and table_name = 'PaymentGatewayPartners') " +
                "set @sql = 'alter table [dbo].[PaymentGatewayPartners] drop constraint [' + @pkName + ']' " +
                "exec(@sql)");

            AlterColumn("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.PaymentGatewayPartners", "Code", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.PaymentGatewayPartners", "Name", c => c.String(nullable: false, maxLength: 150, unicode: false));
            AlterColumn("dbo.PaymentGatewayPartners", "SearchFields", c => c.String(maxLength: 1000));
            AddPrimaryKey("dbo.PaymentGatewayPartners", "Code");
            CreateIndex("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode");
            AddForeignKey("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", "dbo.PaymentGatewayPartners", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", "dbo.PaymentGatewayPartners");
            DropIndex("dbo.TenantAdditionalDatas", new[] { "PaymentGatewayPartnerCode" });
            DropPrimaryKey("dbo.PaymentGatewayPartners");
            AlterColumn("dbo.PaymentGatewayPartners", "SearchFields", c => c.String());
            AlterColumn("dbo.PaymentGatewayPartners", "Name", c => c.String());
            AlterColumn("dbo.PaymentGatewayPartners", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.PaymentGatewayPartners", "Code");
            CreateIndex("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode");
            AddForeignKey("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", "dbo.PaymentGatewayPartners", "Code");
        }
    }
}
