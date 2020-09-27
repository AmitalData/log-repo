namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReferantExceptionReason : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.ExceptionReasons",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4),
                        Tenant = c.Int(nullable: false),
                        EnglishName = c.String(maxLength: 100, unicode: false),
                        LocalName = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        UnifreightStatusCode = c.String(maxLength: 3, unicode: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.ReferantExceptions",
                c => new
                    {
                        DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        ExceptionReasonsCode = c.String(nullable: false, maxLength: 4),
                        Tenant = c.Int(nullable: false),
                        ExceptionRemarks = c.String(maxLength: 1024),
                        Status = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => new { t.DeclarationId, t.ExceptionReasonsCode })
                .ForeignKey("Customs.ExceptionReasons", t => t.ExceptionReasonsCode)
                .Index(t => t.ExceptionReasonsCode);
            
            AddColumn("Customs.DeclarationReferantDatas", "ExceptionReasonsList", c => c.String(maxLength: 1000, unicode: false));
            DropColumn("Customs.DeclarationReferantDatas", "IsExceptional");
        }
        
        public override void Down()
        {
            AddColumn("Customs.DeclarationReferantDatas", "IsExceptional", c => c.Boolean(nullable: false));
            DropForeignKey("Customs.ReferantExceptions", "ExceptionReasonsCode", "Customs.ExceptionReasons");
            DropIndex("Customs.ReferantExceptions", new[] { "ExceptionReasonsCode" });
            DropColumn("Customs.DeclarationReferantDatas", "ExceptionReasonsList");
            DropColumn("Customs.ConsignmentPackDangers", "ClassificationFourDigit");
            DropTable("Customs.ReferantExceptions");
            DropTable("Customs.ExceptionReasons");
        }
    }
}
