namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddObjectFieldCodeToObjectTableRuleFieldsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObjectTableRuleFields", "ObjectFieldCode", c => c.String(nullable: true, maxLength: 200, unicode: false));
            Sql(@"update ObjectTableRuleFields set ObjectFieldCode = (select ObjectFields.FieldCode from ObjectFields where id=ObjectTableRuleFields.ObjectFieldId)");
            AlterColumn("dbo.ObjectTableRuleFields", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }

        public override void Down()
        {
            DropColumn("dbo.ObjectTableRuleFields", "ObjectFieldCode");
        }
    }
}
