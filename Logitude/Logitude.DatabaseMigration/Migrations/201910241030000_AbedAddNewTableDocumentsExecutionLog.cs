namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddNewTableDocumentsExecutionLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentsExecutionLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 40, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        StatusCode = c.String(nullable: false, maxLength: 4, unicode: false),
                        ExceptionMessage = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        DoneDate = c.DateTime(),
                        RequestXML = c.String(),
                        DocumentTypeId = c.String(maxLength: 15, unicode: false),
                        DocumentTypeTemplateId = c.String(maxLength: 15, unicode: false),
                        RetryNumber = c.Int(nullable: false),
                        StartDate = c.DateTime(),
                        Logs = c.String(),
                        Subject = c.String(nullable: false, maxLength: 40),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CommunicationStatusTypes", t => t.StatusCode)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.StatusCode);
            

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentsExecutionLogs", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.DocumentsExecutionLogs", "StatusCode", "dbo.CommunicationStatusTypes");
            DropIndex("dbo.DocumentsExecutionLogs", new[] { "StatusCode" });
            DropIndex("dbo.DocumentsExecutionLogs", new[] { "CreatedByUserId" });
            DropTable("dbo.DocumentsExecutionLogs");
        }
    }
}
