namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingcustoms2905_4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.MamanSpecialActions",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 2, unicode: false),
                        LocalName = c.String(maxLength: 40),
                        SearchFields = c.String(maxLength: 1000),
                        EnglishName = c.String(maxLength: 40, unicode: false),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.MamanSpecialActionStatuses",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 3, unicode: false),
                        LocalName = c.String(maxLength: 40),
                        SearchFields = c.String(maxLength: 1000),
                        EnglishName = c.String(maxLength: 40, unicode: false),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.DeclarationMamanSpecialActions",
                c => new
                    {
                        DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        MamanSpecialActionCode = c.String(nullable: false, maxLength: 2, unicode: false),
                        Tenant = c.Int(nullable: false),
                        MamanLabelText1 = c.String(maxLength: 40),
                        MamanLabelText2 = c.String(maxLength: 40),
                        MamanLabelText3 = c.String(maxLength: 40),
                        MamanLabelText4 = c.String(maxLength: 40),
                        MamanLabelText5 = c.String(maxLength: 40),
                        MamanSpecialActionStatusCode = c.String(maxLength: 3, unicode: false),
                        MamanSpecialActionsErrorXml = c.String(maxLength: 1200),
                    })
                .PrimaryKey(t => new { t.DeclarationId, t.MamanSpecialActionCode })
                .ForeignKey("Customs.Declarations", t => t.DeclarationId)
                .ForeignKey("Customs.MamanSpecialActions", t => t.MamanSpecialActionCode)
                .ForeignKey("Customs.MamanSpecialActionStatuses", t => t.MamanSpecialActionStatusCode)
                .Index(t => t.DeclarationId)
                .Index(t => t.MamanSpecialActionCode)
                .Index(t => t.MamanSpecialActionStatusCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.DeclarationMamanSpecialActions", "MamanSpecialActionStatusCode", "Customs.MamanSpecialActionStatuses");
            DropForeignKey("Customs.DeclarationMamanSpecialActions", "MamanSpecialActionCode", "Customs.MamanSpecialActions");
            DropForeignKey("Customs.DeclarationMamanSpecialActions", "DeclarationId", "Customs.Declarations");
            DropIndex("Customs.DeclarationMamanSpecialActions", new[] { "MamanSpecialActionStatusCode" });
            DropIndex("Customs.DeclarationMamanSpecialActions", new[] { "MamanSpecialActionCode" });
            DropIndex("Customs.DeclarationMamanSpecialActions", new[] { "DeclarationId" });
            DropTable("Customs.DeclarationMamanSpecialActions");
            DropTable("Customs.MamanSpecialActionStatuses");
            DropTable("Customs.MamanSpecialActions");
        }
    }
}
