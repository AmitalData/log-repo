namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddObjectFieldCodeToRuleConditionFieldsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RuleConditionFields", "ObjectFieldCode", c => c.String(nullable: true, maxLength: 200, unicode: false));
            Sql(@"update RuleConditionFields set ObjectFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = RuleConditionFields.ObjectFieldId)");
            AlterColumn("dbo.RuleConditionFields", "ObjectFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }

        public override void Down()
        {
            DropColumn("dbo.RuleConditionFields", "ObjectFieldCode");
        }
    }
}
