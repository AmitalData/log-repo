namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeclarationCourierMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.CourierStatuses",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 1, unicode: false),
                        LocalName = c.String(maxLength: 100, unicode: false),
                        SearchFields = c.String(),
                        EnglishName = c.String(maxLength: 100),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.DeclarationCourierStatuses",
                c => new
                    {
                        DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CourierManifestStatusCode = c.String(maxLength: 1, unicode: false),
                        CourierDeclarationStatusCode = c.String(maxLength: 1, unicode: false),
                        CourierPaymentStatusCode = c.String(maxLength: 1, unicode: false),
                        IsCourierMissingClassification = c.Boolean(nullable: false),
                        IsClosedForFollowUp = c.Boolean(nullable: false),
                        HighLowValue = c.String(maxLength: 1, unicode: false),
                        DocumentStatusCode = c.String(maxLength: 1, unicode: false),
                        TotalInvoiceAmountInUSD = c.Decimal(precision: 16, scale: 2),
                        CourierPendingReasonCode = c.String(maxLength: 128),
                        PendingRemarks = c.String(maxLength: 1024),
                    })
                .PrimaryKey(t => t.DeclarationId)
                .ForeignKey("dbo.CourierPendingReasons", t => t.CourierPendingReasonCode)
                .ForeignKey("Customs.Declarations", t => t.DeclarationId)
                .Index(t => t.DeclarationId)
                .Index(t => t.CourierPendingReasonCode);
            
            CreateTable(
                "dbo.CourierPendingReasons",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        LocalName = c.String(),
                        SearchFields = c.String(),
                        EnglishName = c.String(),
                        Inactive = c.Boolean(nullable: false),
                        ErrorPlace = c.String(),
                        Tenant = c.Int(nullable: false),
                        UnifreightStatusCode = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.DeclarationCourierStatuses", "DeclarationId", "Customs.Declarations");
            DropForeignKey("Customs.DeclarationCourierStatuses", "CourierPendingReasonCode", "dbo.CourierPendingReasons");
            DropIndex("Customs.DeclarationCourierStatuses", new[] { "CourierPendingReasonCode" });
            DropIndex("Customs.DeclarationCourierStatuses", new[] { "DeclarationId" });
            DropTable("dbo.CourierPendingReasons");
            DropTable("Customs.DeclarationCourierStatuses");
            DropTable("Customs.CourierStatuses");
        }
    }
}
