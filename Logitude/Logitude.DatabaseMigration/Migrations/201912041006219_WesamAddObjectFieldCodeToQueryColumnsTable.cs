namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddObjectFieldCodeToQueryColumnsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QueryColumns", "ObjectFieldCode", c => c.String(nullable: true, maxLength: 200, unicode: false));
            Sql(@"update QueryColumns set ObjectFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = QueryColumns.ObjectFieldId)");
            AlterColumn("dbo.QueryColumns", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));

        }
        
        public override void Down()
        {
            DropColumn("dbo.QueryColumns", "ObjectFieldCode");
        }
    }
}
