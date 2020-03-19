namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddRuleObjectCodeTonAirlineMessagingRulesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AirlineMessagingRules", "RuleFieldCode", c => c.String(nullable: true, maxLength: 200, unicode: false));
            Sql(@"update AirlineMessagingRules set RuleFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = AirlineMessagingRules.RuleFieldId)");
            AlterColumn("dbo.AirlineMessagingRules", "RuleFieldCode", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }

        public override void Down()
        {
            DropColumn("dbo.AirlineMessagingRules", "RuleFieldCode");
        }
    }
}
