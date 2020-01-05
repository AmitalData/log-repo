namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSupportMailboxTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SupportMailboxes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Mailbox = c.String(nullable: false, maxLength: 100, unicode: false),
                        Inactive = c.Boolean(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupportMailboxes", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.SupportMailboxes", "CreatedByUserId", "dbo.Users");
            DropIndex("dbo.SupportMailboxes", new[] { "UpdatedByUserId" });
            DropIndex("dbo.SupportMailboxes", new[] { "CreatedByUserId" });
            DropTable("dbo.SupportMailboxes");
        }
    }
}
