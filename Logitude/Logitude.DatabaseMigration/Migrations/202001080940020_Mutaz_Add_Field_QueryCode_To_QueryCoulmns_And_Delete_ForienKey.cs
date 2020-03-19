namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Add_Field_QueryCode_To_QueryCoulmns_And_Delete_ForienKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QueryColumns", "QueryCode", c => c.String(nullable: true, maxLength: 30, unicode: false));
             Sql(@"update QueryColumns set QueryCode = (select Queries.Code from Queries where Id = QueryColumns.QueryId)");
            AlterColumn("dbo.QueryColumns", "QueryCode", c => c.String(nullable: false, maxLength: 30, unicode: false));
            Sql("ALTER TABLE QueryColumns DROP CONSTRAINT  FK_QueryColumnQuery");
        }
        
        public override void Down()
        {
            DropColumn("dbo.QueryColumns", "QueryCode");
        }
    }
}
