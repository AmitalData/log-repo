namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeWebHookIdToString_Rabaia : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.WebhookKeys");
            //Sql("Delete from dbo.WebhookKeys");
            //AlterColumn("dbo.WebhookKeys", "Id", c => c.Long(identity: false,nullable:false));
            DropColumn("dbo.WebhookKeys", "Id");
            AddColumn("dbo.WebhookKeys", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            
            //DropColumn()
            //AlterColumn("dbo.WebhookKeys", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AddPrimaryKey("dbo.WebhookKeys", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.WebhookKeys");
            AlterColumn("dbo.WebhookKeys", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.WebhookKeys", "Id");
        }
    }
}
