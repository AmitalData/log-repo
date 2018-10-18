namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LatestSnapShot20181017 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ActivityOwnerHistories", new[] { "OwnerId" });
            CreateTable(
                "dbo.AcceptanceStatus",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        SearchFields = c.String(),
                        LocalName = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.MamanStatus",
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
            
            AlterColumn("dbo.ActivityOwnerHistories", "OwnerId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.ActivityOwnerHistories", "OwnerId");
            CreateIndex("Customs.Declarations", "AcceptanceStatusCode");
            CreateIndex("Customs.Declarations", "MamanStatusCode");
            AddForeignKey("Customs.Declarations", "AcceptanceStatusCode", "dbo.AcceptanceStatus", "Code");
            AddForeignKey("Customs.Declarations", "MamanStatusCode", "dbo.MamanStatus", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JournalMoreDatas", "JournalId", "dbo.Journals");
            DropForeignKey("Customs.Declarations", "MamanStatusCode", "dbo.MamanStatus");
            DropForeignKey("Customs.Declarations", "AcceptanceStatusCode", "dbo.AcceptanceStatus");
            DropIndex("dbo.JournalMoreDatas", new[] { "JournalId" });
            DropIndex("Customs.Declarations", new[] { "MamanStatusCode" });
            DropIndex("Customs.Declarations", new[] { "AcceptanceStatusCode" });
            DropIndex("dbo.ActivityOwnerHistories", new[] { "OwnerId" });
            AlterColumn("dbo.ActivityOwnerHistories", "OwnerId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            
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
            CreateIndex("dbo.ActivityOwnerHistories", "OwnerId");
        }
    }
}
