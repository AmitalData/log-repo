namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OpenFormatForeignKeysMigration : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.OpenFormatReportStatus", newName: "OpenFormatReportStatuses");
            DropForeignKey("dbo.OpenFormatReports", "DateTypeCode", "dbo.OpenFormatDateTypes");
            DropForeignKey("dbo.OpenFormatReports", "StatusTypeCode", "dbo.OpenFormatReportStatus");
            DropIndex("dbo.OpenFormatReports", new[] { "DateTypeCode" });
            DropIndex("dbo.OpenFormatReports", new[] { "StatusTypeCode" });
            DropPrimaryKey("dbo.OpenFormatDateTypes");
            DropPrimaryKey("dbo.OpenFormatReportStatuses");
           // AddColumn("dbo.Tickets", "EntityType", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.OpenFormatReports", "DateTypeCode", c => c.String(maxLength: 1, unicode: false));
            AlterColumn("dbo.OpenFormatReports", "StatusTypeCode", c => c.String(maxLength: 1, unicode: false));
            AlterColumn("dbo.OpenFormatDateTypes", "Code", c => c.String(nullable: false, maxLength: 1, unicode: false));
            AlterColumn("dbo.OpenFormatDateTypes", "EnglishName", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.OpenFormatDateTypes", "LocalName", c => c.String(maxLength: 100));
            AlterColumn("dbo.OpenFormatReportStatuses", "Code", c => c.String(nullable: false, maxLength: 1, unicode: false));
            AlterColumn("dbo.OpenFormatReportStatuses", "EnglishName", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.OpenFormatReportStatuses", "LocalName", c => c.String(maxLength: 100));
            AddPrimaryKey("dbo.OpenFormatDateTypes", "Code");
            AddPrimaryKey("dbo.OpenFormatReportStatuses", "Code");
           // CreateIndex("dbo.Tickets", "EntityType");
            CreateIndex("dbo.OpenFormatReports", "DateTypeCode");
            CreateIndex("dbo.OpenFormatReports", "StatusTypeCode");
            //AddForeignKey("dbo.Tickets", "EntityType", "dbo.ObjectTables", "Id");
            AddForeignKey("dbo.OpenFormatReports", "DateTypeCode", "dbo.OpenFormatDateTypes", "Code");
            AddForeignKey("dbo.OpenFormatReports", "StatusTypeCode", "dbo.OpenFormatReportStatuses", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OpenFormatReports", "StatusTypeCode", "dbo.OpenFormatReportStatuses");
            DropForeignKey("dbo.OpenFormatReports", "DateTypeCode", "dbo.OpenFormatDateTypes");
            DropForeignKey("dbo.Tickets", "EntityType", "dbo.ObjectTables");
            DropIndex("dbo.OpenFormatReports", new[] { "StatusTypeCode" });
            DropIndex("dbo.OpenFormatReports", new[] { "DateTypeCode" });
            DropIndex("dbo.Tickets", new[] { "EntityType" });
            DropPrimaryKey("dbo.OpenFormatReportStatuses");
            DropPrimaryKey("dbo.OpenFormatDateTypes");
            AlterColumn("dbo.OpenFormatReportStatuses", "LocalName", c => c.String());
            AlterColumn("dbo.OpenFormatReportStatuses", "EnglishName", c => c.String());
            AlterColumn("dbo.OpenFormatReportStatuses", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.OpenFormatDateTypes", "LocalName", c => c.String());
            AlterColumn("dbo.OpenFormatDateTypes", "EnglishName", c => c.String());
            AlterColumn("dbo.OpenFormatDateTypes", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.OpenFormatReports", "StatusTypeCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.OpenFormatReports", "DateTypeCode", c => c.String(maxLength: 128));
            DropColumn("dbo.Tickets", "EntityType");
            AddPrimaryKey("dbo.OpenFormatReportStatuses", "Code");
            AddPrimaryKey("dbo.OpenFormatDateTypes", "Code");
            CreateIndex("dbo.OpenFormatReports", "StatusTypeCode");
            CreateIndex("dbo.OpenFormatReports", "DateTypeCode");
            AddForeignKey("dbo.OpenFormatReports", "StatusTypeCode", "dbo.OpenFormatReportStatus", "Code");
            AddForeignKey("dbo.OpenFormatReports", "DateTypeCode", "dbo.OpenFormatDateTypes", "Code");
            RenameTable(name: "dbo.OpenFormatReportStatuses", newName: "OpenFormatReportStatus");
        }
    }
}
