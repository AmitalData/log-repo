namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWebHookKeysTable_Rabaia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WebhookKeys",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AccessKey = c.String(maxLength: 200, unicode: false),
                        Tenant = c.Int(nullable: false),
                        PartnerName = c.String(nullable: false, maxLength: 150, unicode: false),
                        InActive = c.Boolean(nullable: false),
                        CreatedByUserName = c.String(nullable: false, maxLength: 150, unicode: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdatedByUserName = c.String(nullable: false, maxLength: 150, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Description = c.String(maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WebhookKeys");
        }
    }
}
