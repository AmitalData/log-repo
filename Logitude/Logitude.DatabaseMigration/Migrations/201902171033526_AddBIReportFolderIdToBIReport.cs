namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBIReportFolderIdToBIReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BIReports", "BIReportFolderId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.BIReports", "BIReportFolderId");
            AddForeignKey("dbo.BIReports", "BIReportFolderId", "dbo.BIReportFolders", "Id");

            Sql(@"update BIReports set BIReportFolderId = (select Id from BIReportFolders where Name = 'General' and Tenant = BIReports.Tenant)");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BIReports", "BIReportFolderId", "dbo.BIReportFolders");
            DropIndex("dbo.BIReports", new[] { "BIReportFolderId" });
            DropColumn("dbo.BIReports", "BIReportFolderId");
        }
    }
}
