namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteOpenFormatRportDatetypeMigration : DbMigration
    {
        public override void Up()
        {
           // DropForeignKey("dbo.OpenFormatReports", "DateTypeCode", "dbo.OpenFormatDateTypes");
           // DropIndex("dbo.OpenFormatReports", new[] { "DateTypeCode" });
           // DropColumn("dbo.OpenFormatReports", "DateTypeCode");
            //DropTable("dbo.OpenFormatDateTypes");

            Sql("delete from QueryColumns where ObjectFieldId=(select ID from ObjectFields where FieldName ='DateTypeCode' and ObjectTableId = (select ID from ObjectTables where Name='OpenFormatReport'))");
            Sql("delete from QueryColumns where ObjectFieldId=(select ID from ObjectFields where FieldName ='DateTypeName' and ObjectTableId = (select ID from ObjectTables where Name='OpenFormatReport'))");


            Sql("delete from ScreenFields where ObjectFieldId =(select ID from ObjectFields where FieldName ='DateTypeCode' and ObjectTableId = (select ID from ObjectTables where Name='OpenFormatReport'))");
            Sql("delete from ScreenFields where ObjectFieldId =(select ID from ObjectFields where FieldName ='DateTypeName' and ObjectTableId = (select ID from ObjectTables where Name='OpenFormatReport'))");

            Sql("delete from AdvancedQueryFilters where objectfieldId=(select id from objectfields where fieldname='DateTypeCode' and ObjecttableId=(select id from objecttables where name='OpenFormatReport'))");


            Sql("delete  from ObjectFields where FieldName='DateTypeCode' and ObjectTableId = (select ID from ObjectTables where Name='OpenFormatReport')");
            Sql("delete  from ObjectFields where FieldName='DateTypeName' and ObjectTableId = (select ID from ObjectTables where Name='OpenFormatReport')");



        }

        public override void Down()
        {
        }
    }
}
