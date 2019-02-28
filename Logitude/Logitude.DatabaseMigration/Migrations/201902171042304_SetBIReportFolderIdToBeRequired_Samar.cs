namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetBIReportFolderIdToBeRequired_Samar : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BIReports", new[] { "BIReportFolderId" });
            AlterColumn("dbo.BIReports", "BIReportFolderId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            CreateIndex("dbo.BIReports", "BIReportFolderId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BIReports", new[] { "BIReportFolderId" });
            AlterColumn("dbo.BIReports", "BIReportFolderId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.BIReports", "BIReportFolderId");
        }
    }
}
