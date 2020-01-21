namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddTriggerFieldCodeToObjectTableRuleTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObjectTableRules", "TriggerFieldCode", c => c.String(maxLength: 200, unicode: false));
            Sql(@"update ObjectTableRules set TriggerFieldCode =(select ObjectFields.FieldCode from ObjectFields where id =ObjectTableRules.TriggerFieldId)");
        }
        
        public override void Down()
        {
            DropColumn("dbo.ObjectTableRules", "TriggerFieldCode");
        }
    }
}
