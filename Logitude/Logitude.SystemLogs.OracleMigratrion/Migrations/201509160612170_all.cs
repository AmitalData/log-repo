namespace Logitude.SystemLogs.OracleMigratrion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class all : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ErrorLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 40, unicode: false),
                        Tenant = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 100, unicode: false),
                        LogDate = c.DateTime(nullable: false),
                        ClientDate = c.DateTime(nullable: false),
                        Tier = c.String(nullable: false, maxLength: 40, unicode: false),
                        Exception = c.String(nullable: false, unicode: false),
                        StackTrace = c.String(unicode: false),
                        SearchFields = c.String(unicode: false),
                        IP = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContactActivityLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 40, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Module = c.String(nullable: false, maxLength: 100, unicode: false),
                        Activity = c.String(nullable: false, maxLength: 150, unicode: false),
                        ContactId = c.String(nullable: false, maxLength: 15, unicode: false),
                        LogDateTime = c.DateTime(nullable: false),
                        GMTLogDateTime = c.DateTime(nullable: false),
                        IsSharedLogisticsContact = c.Boolean(nullable: false),
                        CardId = c.String(maxLength: 15, unicode: false),
                        PartnerTypeId = c.String(maxLength: 2, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ContactActivityLogs");
            DropTable("dbo.ErrorLogs");
        }
    }
}
