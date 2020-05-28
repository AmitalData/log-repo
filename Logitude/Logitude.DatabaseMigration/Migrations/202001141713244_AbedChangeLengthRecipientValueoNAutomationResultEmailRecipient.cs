namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedChangeLengthRecipientValueoNAutomationResultEmailRecipient : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AutomationResultEmailRecipients", "RecipientValue", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AutomationResultEmailRecipients", "RecipientValue", c => c.String(maxLength: 15, unicode: false));
            
        }
    }
}
