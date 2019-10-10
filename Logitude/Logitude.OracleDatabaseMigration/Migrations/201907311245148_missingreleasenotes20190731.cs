namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingreleasenotes20190731 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsersReleaseNotesDisplays",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersReleaseNotesDisplays", "UserId", "dbo.Users");
            DropIndex("dbo.UsersReleaseNotesDisplays", new[] { "UserId" });
            DropTable("dbo.UsersReleaseNotesDisplays");
        }
    }
}
