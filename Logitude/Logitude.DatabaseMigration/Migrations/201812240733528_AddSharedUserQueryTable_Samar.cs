namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSharedUserQueryTable_Samar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SharedUserQueries",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        QueryId = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Queries", t => t.QueryId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.QueryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SharedUserQueries", "UserId", "dbo.Users");
            DropForeignKey("dbo.SharedUserQueries", "QueryId", "dbo.Queries");
            DropIndex("dbo.SharedUserQueries", new[] { "QueryId" });
            DropIndex("dbo.SharedUserQueries", new[] { "UserId" });
            DropTable("dbo.SharedUserQueries");
        }
    }
}
