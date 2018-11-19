namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OpenFormatClosedTablesMigration : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.OpenFormatReports",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            CreateDate = c.DateTime(nullable: false),
            //            CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            UpdateDate = c.DateTime(nullable: false),
            //            UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            SearchFields = c.String(),
            //            ReportNumber = c.String(maxLength: 15, unicode: false),
            //            FromDate = c.DateTime(),
            //            ToDate = c.DateTime(),
            //            DateTypeCode = c.String(maxLength: 128),
            //            StatusTypeCode = c.String(maxLength: 128),
            //            ErrorMessage = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Users", t => t.CreatedByUserId)
            //    .ForeignKey("dbo.OpenFormatDateTypes", t => t.DateTypeCode)
            //    .ForeignKey("dbo.OpenFormatReportStatus", t => t.StatusTypeCode)
            //    .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
            //    .Index(t => t.CreatedByUserId)
            //    .Index(t => t.UpdatedByUserId)
            //    .Index(t => t.DateTypeCode)
            //    .Index(t => t.StatusTypeCode);
            
            CreateTable(
                "dbo.OpenFormatDateTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        EnglishName = c.String(),
                        SearchFields = c.String(),
                        LocalName = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.OpenFormatReportStatus",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        EnglishName = c.String(),
                        SearchFields = c.String(),
                        LocalName = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            RenameColumn(table: "dbo.OpenFormatReports", name: "DateType", newName: "DateTypeCode");
            //CreateIndex("dbo.OpenFormatReports", "DateTypeCode");
            //AddForeignKey("dbo.OpenFormatReports", "DateTypeCode", "dbo.OpenFormatDateTypes", "Code");
            //CreateIndex("dbo.OpenFormatReports", "StatusTypeCode");
            //AddForeignKey("dbo.OpenFormatReports", "StatusTypeCode", "dbo.OpenFormatReportStatus", "Code");

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OpenFormatReports", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.OpenFormatReports", "StatusTypeCode", "dbo.OpenFormatReportStatus");
            DropForeignKey("dbo.OpenFormatReports", "DateTypeCode", "dbo.OpenFormatDateTypes");
            DropForeignKey("dbo.OpenFormatReports", "CreatedByUserId", "dbo.Users");
            DropIndex("dbo.OpenFormatReports", new[] { "StatusTypeCode" });
            DropIndex("dbo.OpenFormatReports", new[] { "DateTypeCode" });
            DropIndex("dbo.OpenFormatReports", new[] { "UpdatedByUserId" });
            DropIndex("dbo.OpenFormatReports", new[] { "CreatedByUserId" });
            DropTable("dbo.OpenFormatReportStatus");
            DropTable("dbo.OpenFormatDateTypes");
            DropTable("dbo.OpenFormatReports");
        }
    }
}
