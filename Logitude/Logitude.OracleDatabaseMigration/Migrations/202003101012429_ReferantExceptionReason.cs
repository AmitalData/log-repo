namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReferantExceptionReason : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.ConsignmentPackDangers", "ClassificationFourDigit", c => c.String(maxLength: 4));
            AddColumn("Customs.DeclarationReferantDatas", "ExceptionReasonsList", c => c.String(maxLength: 1000, unicode: false));
            DropColumn("Customs.DeclarationReferantDatas", "IsExceptional");
        }
        
        public override void Down()
        {
            AddColumn("Customs.DeclarationReferantDatas", "IsExceptional", c => c.Boolean(nullable: false));
            DropColumn("Customs.DeclarationReferantDatas", "ExceptionReasonsList");
            DropColumn("Customs.ConsignmentPackDangers", "ClassificationFourDigit");
        }
    }
}
