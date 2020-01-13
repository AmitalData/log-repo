namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDecDangersContact : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.DecDangersContacts",
                c => new
                    {
                        DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CompanyName = c.String(nullable: false, maxLength: 70),
                        CompanyCommNumber = c.String(nullable: false, maxLength: 50, unicode: false),
                        CompanyCommTypeCode = c.String(nullable: false, maxLength: 2, unicode: false),
                        ContactName = c.String(nullable: false, maxLength: 70),
                        ContactCommNumber = c.String(nullable: false, maxLength: 50, unicode: false),
                        ContactCommTypeCode = c.String(nullable: false, maxLength: 2, unicode: false),
                        ContactId = c.String(maxLength: 5, unicode: false),
                    })
                .PrimaryKey(t => t.DeclarationId)
                .ForeignKey("Customs.CommunicationTypes", t => t.CompanyCommTypeCode)
                .ForeignKey("Customs.CommunicationTypes", t => t.ContactCommTypeCode)
                .ForeignKey("Customs.Declarations", t => t.DeclarationId)
                .Index(t => t.DeclarationId)
                .Index(t => t.CompanyCommTypeCode)
                .Index(t => t.ContactCommTypeCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.DecDangersContacts", "DeclarationId", "Customs.Declarations");
            DropForeignKey("Customs.DecDangersContacts", "ContactCommTypeCode", "Customs.CommunicationTypes");
            DropForeignKey("Customs.DecDangersContacts", "CompanyCommTypeCode", "Customs.CommunicationTypes");
            DropIndex("Customs.DecDangersContacts", new[] { "ContactCommTypeCode" });
            DropIndex("Customs.DecDangersContacts", new[] { "CompanyCommTypeCode" });
            DropIndex("Customs.DecDangersContacts", new[] { "DeclarationId" });
            DropTable("Customs.DecDangersContacts");
        }
    }
}
