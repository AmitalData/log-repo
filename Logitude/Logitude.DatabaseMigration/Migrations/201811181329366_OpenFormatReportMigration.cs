namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OpenFormatReportMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OpenFormatReports",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SearchFields = c.String(),
                        ReportNumber = c.String(maxLength: 15, unicode: false),
                        FromDate = c.DateTime(),
                        ToDate = c.DateTime(),
                        DateType = c.String(maxLength: 1, unicode: false),
                        StatusTypeCode = c.String(maxLength: 1, unicode: false),
                        ErrorMessage = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
            //AlterColumn("dbo.InsideShipmentPackages", "Reference1", c => c.String(maxLength: 250, unicode: false));
            //AlterColumn("dbo.InsideShipmentPackages", "Reference2", c => c.String(maxLength: 250, unicode: false));
            //AlterColumn("dbo.InsideShipmentPackages", "Reference3", c => c.String(maxLength: 250, unicode: false));
            //AlterColumn("dbo.InsideShipmentPackages", "Reference4", c => c.String(maxLength: 250, unicode: false));
            //AlterColumn("dbo.ShipmentPackages", "Reference1", c => c.String(maxLength: 250, unicode: false));
            //AlterColumn("dbo.ShipmentPackages", "Reference2", c => c.String(maxLength: 250, unicode: false));
            //AlterColumn("dbo.ShipmentPackages", "Reference3", c => c.String(maxLength: 250, unicode: false));
            //AlterColumn("dbo.ShipmentPackages", "Reference4", c => c.String(maxLength: 250, unicode: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OpenFormatReports", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.OpenFormatReports", "CreatedByUserId", "dbo.Users");
            DropIndex("dbo.OpenFormatReports", new[] { "UpdatedByUserId" });
            DropIndex("dbo.OpenFormatReports", new[] { "CreatedByUserId" });
            AlterColumn("dbo.ShipmentPackages", "Reference4", c => c.String(maxLength: 150, unicode: false));
            AlterColumn("dbo.ShipmentPackages", "Reference3", c => c.String(maxLength: 150, unicode: false));
            AlterColumn("dbo.ShipmentPackages", "Reference2", c => c.String(maxLength: 150, unicode: false));
            AlterColumn("dbo.ShipmentPackages", "Reference1", c => c.String(maxLength: 150, unicode: false));
            AlterColumn("dbo.InsideShipmentPackages", "Reference4", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.InsideShipmentPackages", "Reference3", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.InsideShipmentPackages", "Reference2", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.InsideShipmentPackages", "Reference1", c => c.String(maxLength: 50, unicode: false));
            DropTable("dbo.OpenFormatReports");
        }
    }
}
