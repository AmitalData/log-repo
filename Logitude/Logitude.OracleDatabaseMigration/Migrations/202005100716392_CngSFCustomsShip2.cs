namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CngSFCustomsShip2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("Customs.CustomsShips");
            AlterColumn("Customs.CustomsShips", "Code", c => c.String(nullable: false, maxLength: 25, unicode: false));
            AlterColumn("Customs.CustomsShips", "EnglishName", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("Customs.CustomsShips", "SearchFields", c => c.String(maxLength: 500));
            AlterColumn("Customs.CustomsShips", "LocalName", c => c.String(maxLength: 100));
            AddPrimaryKey("Customs.CustomsShips", "Code");
            //DropTable("Customs.AmountTypes");
            //DropTable("Customs.ClaimReasonTypes");
            //DropTable("Customs.ClassificationTypes");
            //DropTable("Customs.PartyRelationshipTypes");
            //DropTable("Customs.TransactionNatureTypes");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "Customs.TransactionNatureTypes",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 4, unicode: false),
            //            EnglishName = c.String(maxLength: 100, unicode: false),
            //            LocalName = c.String(maxLength: 100),
            //            SearchFields = c.String(maxLength: 1000),
            //            Inactive = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            //CreateTable(
            //    "Customs.PartyRelationshipTypes",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 4, unicode: false),
            //            EnglishName = c.String(maxLength: 100, unicode: false),
            //            LocalName = c.String(maxLength: 100),
            //            SearchFields = c.String(maxLength: 1000),
            //            Inactive = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            //CreateTable(
            //    "Customs.ClassificationTypes",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 4, unicode: false),
            //            EnglishName = c.String(maxLength: 100, unicode: false),
            //            LocalName = c.String(maxLength: 100),
            //            SearchFields = c.String(maxLength: 1000),
            //            Inactive = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            //CreateTable(
            //    "Customs.ClaimReasonTypes",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 4, unicode: false),
            //            EnglishName = c.String(maxLength: 100, unicode: false),
            //            LocalName = c.String(maxLength: 100),
            //            SearchFields = c.String(maxLength: 1000),
            //            Inactive = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            //CreateTable(
            //    "Customs.AmountTypes",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 4, unicode: false),
            //            EnglishName = c.String(maxLength: 100, unicode: false),
            //            LocalName = c.String(maxLength: 100),
            //            SearchFields = c.String(maxLength: 1000),
            //            Inactive = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            DropPrimaryKey("Customs.CustomsShips");
            AlterColumn("Customs.CustomsShips", "LocalName", c => c.String(maxLength: 300));
            AlterColumn("Customs.CustomsShips", "SearchFields", c => c.String());
            AlterColumn("Customs.CustomsShips", "EnglishName", c => c.String(maxLength: 300, unicode: false));
            AlterColumn("Customs.CustomsShips", "Code", c => c.String(nullable: false, maxLength: 4, unicode: false));
            AddPrimaryKey("Customs.CustomsShips", "Code");
        }
    }
}
