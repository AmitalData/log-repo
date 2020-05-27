namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserLastSettingsTable_Rabaia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserLastSettings",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        ControlNameSpace = c.String(nullable: false, maxLength: 100, unicode: false),
                        FilterName = c.String(nullable: false, maxLength: 50, unicode: false),
                        FilterValue = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLastSettings", "UserId", "dbo.Users");
            DropIndex("dbo.UserLastSettings", new[] { "UserId" });
            DropTable("dbo.UserLastSettings");
        }
    }
}
