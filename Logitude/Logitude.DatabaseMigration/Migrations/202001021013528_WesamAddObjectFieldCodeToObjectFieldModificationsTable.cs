namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddObjectFieldCodeToObjectFieldModificationsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObjectFieldModifications", "ObjectFieldCode", c => c.String(nullable: true, maxLength: 200, unicode: false));
            Sql(@"update ObjectFieldModifications set ObjectFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = ObjectFieldModifications.ObjectFieldId)");
            AlterColumn("dbo.ObjectFieldModifications", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }

        public override void Down()
        {
            DropColumn("dbo.ObjectFieldModifications", "ObjectFieldCode");
        }
    }
}
