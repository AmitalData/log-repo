namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddSearchFieldsToWebHookKeysTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WebhookKeys", "SearchFields", c => c.String(maxLength: 1000));
            Sql(@"update WebhookKeys set SearchFields=PartnerName+','+Description+','+AccessKey");
        }
        
        public override void Down()
        {
            DropColumn("dbo.WebhookKeys", "SearchFields");
        }
    }
}
