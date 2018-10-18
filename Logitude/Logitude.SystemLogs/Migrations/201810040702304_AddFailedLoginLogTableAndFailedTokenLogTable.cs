namespace Logitude.SystemLogs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFailedLoginLogTableAndFailedTokenLogTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FailedLoginLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        IP = c.String(maxLength: 15, unicode: false),
                        Browser = c.String(maxLength: 40, unicode: false),
                        Email = c.String(maxLength: 70, unicode: false),
                        GMTDateTime = c.DateTime(),
                        UserAgent = c.String(maxLength: 400),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FailedTokenLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        IP = c.String(maxLength: 15, unicode: false),
                        Browser = c.String(maxLength: 40, unicode: false),
                        Token = c.String(nullable: false, maxLength: 100, unicode: false),
                        GMTDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FailedTokenLogs");
            DropTable("dbo.FailedLoginLogs");
        }
    }
}
