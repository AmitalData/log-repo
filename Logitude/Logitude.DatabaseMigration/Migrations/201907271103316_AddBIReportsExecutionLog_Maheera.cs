namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBIReportsExecutionLog_Maheera : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BIReportsExecutionLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        StatusCode = c.String(maxLength: 4, unicode: false),
                        ExceptionMessage = c.String(),
                        DoneDate = c.DateTime(nullable: false),
                        ReportFilterXML = c.String(),
                        BIReportId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CommunicationStatusTypes", t => t.StatusCode)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.StatusCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BIReportsExecutionLogs", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.BIReportsExecutionLogs", "StatusCode", "dbo.CommunicationStatusTypes");
            DropIndex("dbo.BIReportsExecutionLogs", new[] { "StatusCode" });
            DropIndex("dbo.BIReportsExecutionLogs", new[] { "CreatedByUserId" });
            DropTable("dbo.BIReportsExecutionLogs");
        }
    }
}
