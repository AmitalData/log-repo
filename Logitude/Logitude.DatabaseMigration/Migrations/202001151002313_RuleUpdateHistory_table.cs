namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RuleUpdateHistory_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RuleUpdateHistories",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        RuleCode = c.String(maxLength: 40, unicode: false),
                        EventName = c.String(nullable: false, maxLength: 100, unicode: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RuleUpdateHistories", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.RuleUpdateHistories", "CreatedByUserId", "dbo.Users");
            DropIndex("dbo.RuleUpdateHistories", new[] { "UpdatedByUserId" });
            DropIndex("dbo.RuleUpdateHistories", new[] { "CreatedByUserId" });
         
            DropTable("dbo.RuleUpdateHistories");
        }
    }
}
