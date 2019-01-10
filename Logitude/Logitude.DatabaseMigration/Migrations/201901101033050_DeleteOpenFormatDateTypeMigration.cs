namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteOpenFormatDateTypeMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OpenFormatReports", "DateTypeCode", "dbo.OpenFormatDateTypes");
            DropIndex("dbo.OpenFormatReports", new[] { "DateTypeCode" });
            DropColumn("dbo.OpenFormatReports", "DateTypeCode");
            DropTable("dbo.OpenFormatDateTypes");

            Sql("delete from QueryColumns where ObjectFieldId=(select ID from ObjectFields where FieldName ='DateTypeCode' and ObjectTableId = (select ID from ObjectTables where Name='OpenFormatReport'))");
            Sql("delete from QueryColumns where ObjectFieldId=(select ID from ObjectFields where FieldName ='DateTypeName' and ObjectTableId = (select ID from ObjectTables where Name='OpenFormatReport'))");


            Sql("delete from ScreenFields where ObjectFieldId =(select ID from ObjectFields where FieldName ='DateTypeCode' and ObjectTableId = (select ID from ObjectTables where Name='OpenFormatReport'))");
            Sql("delete from ScreenFields where ObjectFieldId =(select ID from ObjectFields where FieldName ='DateTypeName' and ObjectTableId = (select ID from ObjectTables where Name='OpenFormatReport'))");


            Sql("delete  from ObjectFields where FieldName='DateTypeCode' and ObjectTableId = (select ID from ObjectTables where Name='OpenFormatReport')");
            Sql("delete  from ObjectFields where FieldName='DateTypeName' and ObjectTableId = (select ID from ObjectTables where Name='OpenFormatReport')");
        

        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OpenFormatDateTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 1, unicode: false),
                        EnglishName = c.String(maxLength: 100, unicode: false),
                        SearchFields = c.String(),
                        LocalName = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Code);
            
            AddColumn("dbo.OpenFormatReports", "DateTypeCode", c => c.String(nullable: false, maxLength: 1, unicode: false));
            CreateIndex("dbo.OpenFormatReports", "DateTypeCode");
            AddForeignKey("dbo.OpenFormatReports", "DateTypeCode", "dbo.OpenFormatDateTypes", "Code");
        }
    }
}
