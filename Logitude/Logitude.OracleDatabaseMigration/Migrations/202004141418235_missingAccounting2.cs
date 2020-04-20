namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class missingAccounting2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RevaluationStatus", newName: "RevaluationStatuses");
            DropForeignKey("dbo.Revaluations", "Status", "dbo.RevaluationStatus");
            DropIndex("dbo.Revaluations", new[] { "Status" });
            DropPrimaryKey("dbo.RevaluationStatuses");
            CreateTable(
                "dbo.OpenFormatReportStatuses",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 1, unicode: false),
                    EnglishName = c.String(maxLength: 100, unicode: false),
                    SearchFields = c.String(),
                    LocalName = c.String(maxLength: 100),
                })
                .PrimaryKey(t => t.Code);

            CreateTable(
                "dbo.TaxDeductionReportStatuses",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 1, unicode: false),
                    EnglishName = c.String(maxLength: 100, unicode: false),
                    SearchFields = c.String(),
                    LocalName = c.String(maxLength: 30),
                    Inactive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Code);

            CreateTable(
                "dbo.TaxReportLineStatuses",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 3, unicode: false),
                    EnglishName = c.String(maxLength: 100, unicode: false),
                    SearchFields = c.String(),
                    LocalName = c.String(maxLength: 100),
                    Inactive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Code);

            CreateTable(
                "dbo.TaxReportLineTransmitStatuses",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 3, unicode: false),
                    EnglishName = c.String(maxLength: 100, unicode: false),
                    SearchFields = c.String(),
                    LocalName = c.String(maxLength: 100),
                    Inactive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Code);

            CreateTable(
                "dbo.TaxReportLineTypes",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 3, unicode: false),
                    Name = c.String(maxLength: 60),
                    SearchFields = c.String(),
                    Inactive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Code);

            CreateTable(
                "dbo.TaxReportStatuses",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 1, unicode: false),
                    Name = c.String(maxLength: 100, unicode: false),
                    SearchFields = c.String(),
                    LocalName = c.String(maxLength: 100),
                    Inactive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Code);

            CreateTable(
                "dbo.VatReportStatuses",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 3, unicode: false),
                    EnglishName = c.String(maxLength: 100, unicode: false),
                    SearchFields = c.String(),
                    LocalName = c.String(maxLength: 100),
                    Inactive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Code);

            AlterColumn("dbo.Revaluations", "Status", c => c.String(maxLength: 3, unicode: false));
            AlterColumn("dbo.RevaluationStatuses", "Code", c => c.String(nullable: false, maxLength: 3, unicode: false));
            //AlterColumn("dbo.RevaluationStatuses", "Name", c => c.String(maxLength: 100, unicode: false));
            //AlterColumn("dbo.RevaluationStatuses", "LocalName", c => c.String(maxLength: 100));
            AddPrimaryKey("dbo.RevaluationStatuses", "Code");
            CreateIndex("dbo.Revaluations", "Status");
            AddForeignKey("dbo.Revaluations", "Status", "dbo.RevaluationStatuses", "Code");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Revaluations", "Status", "dbo.RevaluationStatuses");
            DropIndex("dbo.Revaluations", new[] { "Status" });
            DropPrimaryKey("dbo.RevaluationStatuses");
            AlterColumn("dbo.RevaluationStatuses", "LocalName", c => c.String());
            AlterColumn("dbo.RevaluationStatuses", "Name", c => c.String());
            AlterColumn("dbo.RevaluationStatuses", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Revaluations", "Status", c => c.String(maxLength: 128));
            DropTable("dbo.VatReportStatuses");
            DropTable("dbo.TaxReportStatuses");
            DropTable("dbo.TaxReportLineTypes");
            DropTable("dbo.TaxReportLineTransmitStatuses");
            DropTable("dbo.TaxReportLineStatuses");
            DropTable("dbo.TaxDeductionReportStatuses");
            DropTable("dbo.OpenFormatReportStatuses");
            AddPrimaryKey("dbo.RevaluationStatuses", "Code");
            CreateIndex("dbo.Revaluations", "Status");
            AddForeignKey("dbo.Revaluations", "Status", "dbo.RevaluationStatus", "Code");
            RenameTable(name: "dbo.RevaluationStatuses", newName: "RevaluationStatus");
        }
    }
}
