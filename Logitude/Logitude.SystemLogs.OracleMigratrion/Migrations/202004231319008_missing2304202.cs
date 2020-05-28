namespace Logitude.SystemLogs.OracleMigratrion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missing2304202 : DbMigration
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
                        GMTDateTime = c.DateTime(precision: 7),
                        UserAgent = c.String(maxLength: 400),
                        Reason = c.String(maxLength: 200, unicode: false),
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
                        GMTDateTime = c.DateTime(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.BatchServicesLogs", "WaitingItems", c => c.Int(nullable: false));
            AddColumn("dbo.BatchServicesLogs", "FailedItems", c => c.Int(nullable: false));
            AddColumn("dbo.BatchServicesLogs", "RelatedQueueMessage", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BatchServicesLogs", "RelatedQueueMessage");
            DropColumn("dbo.BatchServicesLogs", "FailedItems");
            DropColumn("dbo.BatchServicesLogs", "WaitingItems");
            DropTable("dbo.FailedTokenLogs");
            DropTable("dbo.FailedLoginLogs");
        }
    }
}
