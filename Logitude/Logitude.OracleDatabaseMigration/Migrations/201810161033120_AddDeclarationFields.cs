namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeclarationFields : DbMigration
    {
        public override void Up()
        {
            
            CreateTable(
                "dbo.AcceptanceStatuses",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        SearchFields = c.String(),
                        LocalName = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.MamanStatuses",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        SearchFields = c.String(),
                        LocalName = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            AddColumn("Customs.Declarations", "AcceptanceStatusCode", c => c.String(maxLength: 128));
            AddColumn("Customs.Declarations", "CasualImporterAddress1", c => c.String(maxLength: 35));
            AddColumn("Customs.Declarations", "CasualImporterAddress2", c => c.String(maxLength: 35));
            AddColumn("Customs.Declarations", "CasualImporterCity", c => c.String(maxLength: 17));
            AddColumn("Customs.Declarations", "CasualImporterZipCode", c => c.String(maxLength: 10, unicode: false));
            AddColumn("Customs.Declarations", "CasualImporterFax", c => c.String(maxLength: 30, unicode: false));
            AddColumn("Customs.Declarations", "CasualImporterEmail", c => c.String(maxLength: 50, unicode: false));
            AddColumn("Customs.Declarations", "CasualImporterTel", c => c.String(maxLength: 30, unicode: false));
            AddColumn("Customs.Declarations", "CasualImporterContact", c => c.String(maxLength: 50));
            AddColumn("Customs.Declarations", "MamanStatusCode", c => c.String(maxLength: 128));
            AddColumn("Customs.Declarations", "MamanErrorXml", c => c.String(unicode: false));
            AddColumn("Customs.Declarations", "ItemsProcessTypesList", c => c.String(unicode: false));
            AddColumn("Customs.Declarations", "IsClose", c => c.Boolean(nullable: false));
            CreateIndex("Customs.Declarations", "AcceptanceStatusCode");
            CreateIndex("Customs.Declarations", "MamanStatusCode");
            AddForeignKey("Customs.Declarations", "AcceptanceStatusCode", "dbo.AcceptanceStatuses", "Code");
            AddForeignKey("Customs.Declarations", "MamanStatusCode", "dbo.MamanStatuses", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.Declarations", "MamanStatusCode", "dbo.MamanStatus");
            DropForeignKey("Customs.Declarations", "AcceptanceStatusCode", "dbo.AcceptanceStatus");
            DropIndex("Customs.Declarations", new[] { "MamanStatusCode" });
            DropIndex("Customs.Declarations", new[] { "AcceptanceStatusCode" });
            DropColumn("Customs.Declarations", "IsClose");
            DropColumn("Customs.Declarations", "ItemsProcessTypesList");
            DropColumn("Customs.Declarations", "MamanErrorXml");
            DropColumn("Customs.Declarations", "MamanStatusCode");
            DropColumn("Customs.Declarations", "CasualImporterContact");
            DropColumn("Customs.Declarations", "CasualImporterTel");
            DropColumn("Customs.Declarations", "CasualImporterEmail");
            DropColumn("Customs.Declarations", "CasualImporterFax");
            DropColumn("Customs.Declarations", "CasualImporterZipCode");
            DropColumn("Customs.Declarations", "CasualImporterCity");
            DropColumn("Customs.Declarations", "CasualImporterAddress2");
            DropColumn("Customs.Declarations", "CasualImporterAddress1");
            DropColumn("Customs.Declarations", "AcceptanceStatusCode");
            DropTable("dbo.MamanStatus");
            DropTable("dbo.AcceptanceStatus");
            RenameColumn(table: "dbo.EntityChanges", name: "QueuedTaskAutomationSucceedXml", newName: "QueuedTaskAutomationSsucceedXml");
        }
    }
}
