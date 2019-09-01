namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskSchedulerHistory_LogDocumentId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskSchedulerHistory", "LogDocumentId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.TaskSchedulerHistory", "LogDocumentId");
            AddForeignKey("dbo.TaskSchedulerHistory", "LogDocumentId", "dbo.Documents", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskSchedulerHistory", "LogDocumentId", "dbo.Documents");
            DropIndex("dbo.TaskSchedulerHistory", new[] { "LogDocumentId" });
            DropColumn("dbo.TaskSchedulerHistory", "LogDocumentId");
        }
    }
}
