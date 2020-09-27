namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeclarationAmendment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.AmendmentStatuses",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 3, unicode: false),
                        Name = c.String(maxLength: 50, unicode: false),
                        SearchFields = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            AddColumn("Customs.Declarations", "AmendmentRequestNumber", c => c.String(maxLength: 9, unicode: false));
            AddColumn("Customs.Declarations", "AmendmentStatus", c => c.String(maxLength: 3, unicode: false));
            AddColumn("Customs.Declarations", "AmendmentissueDate", c => c.DateTime(precision: 7));
            AddColumn("Customs.Declarations", "AmendmentRemarks", c => c.String(maxLength: 512));
            AddColumn("Customs.Declarations", "AmendmentDeficitInitiated", c => c.Boolean());
            AddColumn("Customs.Declarations", "AmendDeficitInitiatedReasTo", c => c.String(maxLength: 512));
            AddColumn("Customs.Declarations", "AmendmentCorrectedByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("Customs.Declarations", "AmendmentRejectionReason", c => c.String(maxLength: 512));
            AddColumn("Customs.Declarations", "IsAmendment", c => c.Boolean());
            AddColumn("Customs.Declarations", "AmendmentOriginalDeclartation", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("Customs.Declarations", "AmendmentStatus");
            CreateIndex("Customs.Declarations", "AmendmentCorrectedByUserId");
            AddForeignKey("Customs.Declarations", "AmendmentCorrectedByUserId", "dbo.Users", "Id");
            AddForeignKey("Customs.Declarations", "AmendmentStatus", "Customs.AmendmentStatuses", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.Declarations", "AmendmentStatus", "Customs.AmendmentStatuses");
            DropForeignKey("Customs.Declarations", "AmendmentCorrectedByUserId", "dbo.Users");
            DropIndex("Customs.Declarations", new[] { "AmendmentCorrectedByUserId" });
            DropIndex("Customs.Declarations", new[] { "AmendmentStatus" });
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
        }
    }
}
