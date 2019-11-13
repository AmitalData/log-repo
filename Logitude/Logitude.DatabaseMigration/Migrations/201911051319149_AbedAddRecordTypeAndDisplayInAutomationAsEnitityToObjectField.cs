namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddRecordTypeAndDisplayInAutomationAsEnitityToObjectField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObjectFields", "DisplayInAutomationAsEnitity", c => c.Boolean(nullable: false));
            AddColumn("dbo.ObjectFields", "RecordType", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ObjectFields", "RecordType");
            DropColumn("dbo.ObjectFields", "DisplayInAutomationAsEnitity");
        }
    }
}
