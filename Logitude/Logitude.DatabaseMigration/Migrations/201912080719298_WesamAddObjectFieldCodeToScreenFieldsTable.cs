namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddObjectFieldCodeToScreenFieldsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScreenFields", "ObjectFieldCode", c => c.String(nullable: true, maxLength: 200, unicode: false));
            Sql(@"update ScreenFields set ObjectFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = ScreenFields.ObjectFieldId)");
            AlterColumn("dbo.ScreenFields", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScreenFields", "ObjectFieldCode");
        }
    }
}
