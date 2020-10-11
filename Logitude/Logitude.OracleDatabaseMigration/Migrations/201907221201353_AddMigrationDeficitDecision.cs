namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationDeficitDecision : DbMigration
    {
        public override void Up()
        {
            
            DropForeignKey("Customs.DeclarationPendings", "DeclarationID", "Customs.Declarations");
            CreateTable(
                "dbo.ApprovedProfessions",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        LocalName = c.String(),
                        EnglishName = c.String(),
                        Inactive = c.Boolean(nullable: false),
                        SearchFields = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.DeficitDecisions",
                c => new
                    {
                        DeficitId = c.String(nullable: false, maxLength: 15, unicode: false),
                        DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        RequestDate = c.DateTime(precision: 7),
                        RequestID = c.String(maxLength: 9, unicode: false),
                        RequestTypeCode = c.String(maxLength: 2, unicode: false),
                        ApprovedProfessionCode = c.String(maxLength: 128),
                        DecisionCode = c.String(maxLength: 2, unicode: false),
                        DecisionNoteForLetter = c.String(maxLength: 512),
                        TotalComponentAmount = c.Decimal(precision: 16, scale: 2),
                        TotalEstimatedAmount = c.Decimal(precision: 16, scale: 2),
                        TotalFinancialPenaltyAmount = c.Decimal(precision: 16, scale: 2),
                        TotalInterestAmount = c.Decimal(precision: 16, scale: 2),
                        TotalLinkingAmount = c.Decimal(precision: 16, scale: 2),
                    })
                .PrimaryKey(t => new { t.DeficitId, t.DeclarationId })
                .ForeignKey("dbo.ApprovedProfessions", t => t.ApprovedProfessionCode)
                .ForeignKey("Customs.DecisionTypes", t => t.DecisionCode)
                .ForeignKey("Customs.Declarations", t => t.DeclarationId)
                .ForeignKey("Customs.Deficits", t => t.DeficitId)
                .ForeignKey("Customs.RequestTypes", t => t.RequestTypeCode)
                .Index(t => t.DeficitId)
                .Index(t => t.DeclarationId)
                .Index(t => t.RequestTypeCode)
                .Index(t => t.ApprovedProfessionCode)
                .Index(t => t.DecisionCode);
            
            CreateTable(
                "Customs.RequestTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 2, unicode: false),
                        LocalName = c.String(maxLength: 100),
                        EnglishName = c.String(maxLength: 100, unicode: false),
                        Inactive = c.Boolean(nullable: false),
                        SearchFields = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Code);
            
            AddForeignKey("Customs.DeclarationPendings", "DeclarationID", "Customs.DeclarationCourierStatuses", "DeclarationId");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.DeficitDecisions", "RequestTypeCode", "Customs.RequestTypes");
            DropForeignKey("Customs.DeficitDecisions", "DeficitId", "Customs.Deficits");
            DropForeignKey("Customs.DeficitDecisions", "DeclarationId", "Customs.Declarations");
            DropForeignKey("Customs.DeficitDecisions", "DecisionCode", "Customs.DecisionTypes");
            DropForeignKey("Customs.DeficitDecisions", "ApprovedProfessionCode", "dbo.ApprovedProfessions");
            DropForeignKey("Customs.DeclarationPendings", "DeclarationID", "Customs.DeclarationCourierStatuses");
            DropIndex("Customs.DeficitDecisions", new[] { "DecisionCode" });
            DropIndex("Customs.DeficitDecisions", new[] { "ApprovedProfessionCode" });
            DropIndex("Customs.DeficitDecisions", new[] { "RequestTypeCode" });
            DropIndex("Customs.DeficitDecisions", new[] { "DeclarationId" });
            DropIndex("Customs.DeficitDecisions", new[] { "DeficitId" });
            DropTable("Customs.RequestTypes");
            DropTable("Customs.DeficitDecisions");
            DropTable("dbo.ApprovedProfessions");
            AddForeignKey("Customs.DeclarationPendings", "DeclarationID", "Customs.Declarations", "Id");
        }
    }
}
