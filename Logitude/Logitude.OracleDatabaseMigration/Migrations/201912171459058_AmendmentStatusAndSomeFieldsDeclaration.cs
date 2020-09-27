namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AmendmentStatusAndSomeFieldsDeclaration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Customs.DBMigrationLines", "DBMigrationId", "Customs.DBMigrations");
            DropIndex("Customs.DBMigrationLines", new[] { "DBMigrationId" });
            CreateTable(
                "Customs.AmendmentStatuses",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 3, unicode: false),
                        Name = c.String(maxLength: 3, unicode: false),
                        SearchFields = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            AddColumn("Customs.Declarations", "AmendmentRequestNumber", c => c.String(maxLength: 9, unicode: false));
            AddColumn("Customs.Declarations", "AmendmentStatus", c => c.String(maxLength: 2, unicode: false));
            AddColumn("Customs.Declarations", "AmendmentissueDate", c => c.DateTime(precision: 7));
            AddColumn("Customs.Declarations", "AmendmentRemarks", c => c.String(maxLength: 512));
            AddColumn("Customs.Declarations", "AmendmentDeficitInitiated", c => c.Boolean());
            AddColumn("Customs.Declarations", "AmendDeficitInitiatedReasTo", c => c.String(maxLength: 512));
            AddColumn("Customs.Declarations", "AmendmentCorrectedByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("Customs.Declarations", "AmendmentRejectionReason", c => c.String(maxLength: 512));
            AddColumn("Customs.Declarations", "IsAmendment", c => c.Boolean());
            AddColumn("Customs.Declarations", "AmendmentOriginalDeclartation", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("Customs.SupplierInvoiceItems", "MarksAndNumbers", c => c.String(maxLength: 512, unicode: false));
            AlterColumn("Customs.SupplierInvoiceItems", "Weight", c => c.Decimal(precision: 18, scale: 2));
            CreateIndex("Customs.Declarations", "AmendmentCorrectedByUserId");
            AddForeignKey("Customs.Declarations", "AmendmentCorrectedByUserId", "dbo.Users", "Id");
            DropTable("Customs.DBMigrationLines");
            DropTable("Customs.DBMigrations");
        }
        
        public override void Down()
        {
            CreateTable(
                "Customs.DBMigrations",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        ExecuteDate = c.DateTime(nullable: false, precision: 7),
                        MajorVersion = c.Decimal(nullable: false, precision: 5, scale: 2),
                        MinorVersion = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 256),
                        IsClose = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Customs.DBMigrationLines",
                c => new
                    {
                        DBMigrationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        CounterKey = c.Int(nullable: false),
                        SqlScript = c.String(nullable: false, maxLength: 1024, unicode: false),
                        ApprovedRemarks = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => new { t.DBMigrationId, t.CounterKey });
            
            DropForeignKey("Customs.Declarations", "AmendmentCorrectedByUserId", "dbo.Users");
            DropIndex("Customs.Declarations", new[] { "AmendmentCorrectedByUserId" });
            AlterColumn("Customs.SupplierInvoiceItems", "Weight", c => c.Decimal(precision: 15, scale: 3));
            AlterColumn("Customs.SupplierInvoiceItems", "MarksAndNumbers", c => c.String(maxLength: 30, unicode: false));
            DropColumn("Customs.Declarations", "AmendmentOriginalDeclartation");
            DropColumn("Customs.Declarations", "IsAmendment");
            DropColumn("Customs.Declarations", "AmendmentRejectionReason");
            DropColumn("Customs.Declarations", "AmendmentCorrectedByUserId");
            DropColumn("Customs.Declarations", "AmendDeficitInitiatedReasTo");
            DropColumn("Customs.Declarations", "AmendmentDeficitInitiated");
            DropColumn("Customs.Declarations", "AmendmentRemarks");
            DropColumn("Customs.Declarations", "AmendmentissueDate");
            DropColumn("Customs.Declarations", "AmendmentStatus");
            DropColumn("Customs.Declarations", "AmendmentRequestNumber");
            DropTable("Customs.AmendmentStatuses");
            CreateIndex("Customs.DBMigrationLines", "DBMigrationId");
            AddForeignKey("Customs.DBMigrationLines", "DBMigrationId", "Customs.DBMigrations", "Id");
        }
    }
}
