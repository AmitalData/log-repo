namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeclarationExportRecipientMap : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.DeclarationExportRecipients",
                c => new
                    {
                        DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        LineNumber = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        RecipientName = c.String(maxLength: 35, unicode: false),
                        RecipientAddress = c.String(maxLength: 35, unicode: false),
                        RecipientIssueCountryCode = c.String(maxLength: 2, unicode: false),
                    })
                .PrimaryKey(t => new { t.DeclarationId, t.LineNumber })
                .ForeignKey("Customs.CustomsCountries", t => t.RecipientIssueCountryCode)
                .ForeignKey("Customs.Declarations", t => t.DeclarationId)
                .Index(t => t.DeclarationId)
                .Index(t => t.RecipientIssueCountryCode);
            
        }
        
        public override void Down()
        {
           
            DropForeignKey("Customs.DeclarationExportRecipients", "DeclarationId", "Customs.Declarations");
            DropForeignKey("Customs.DeclarationExportRecipients", "RecipientIssueCountryCode", "Customs.CustomsCountries");
            DropIndex("Customs.DeclarationExportRecipients", new[] { "RecipientIssueCountryCode" });
            DropIndex("Customs.DeclarationExportRecipients", new[] { "DeclarationId" });
            DropTable("Customs.DeclarationExportRecipients");
            
        }
    }
}
