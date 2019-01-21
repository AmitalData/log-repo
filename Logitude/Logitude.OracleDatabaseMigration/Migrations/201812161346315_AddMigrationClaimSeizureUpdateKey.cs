namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationClaimSeizureUpdateKey : DbMigration
    {
        public override void Up()
        {
            DropIndex("Customs.DeclarationMamanSpecialActions", new[] { "MamanSpecialActionCode" });
            DropPrimaryKey("Customs.DeclarationMamanSpecialActions");
            AlterColumn("Customs.DeclarationMamanSpecialActions", "MamanSpecialActionCode", c => c.String(nullable: false, maxLength: 2, unicode: false));
            AddPrimaryKey("Customs.DeclarationMamanSpecialActions", new[] { "DeclarationId", "MamanSpecialActionCode" });
            CreateIndex("Customs.ClaimsRelatedEntitiesSeizures", "SeizureFactorCode");
            CreateIndex("Customs.DeclarationMamanSpecialActions", "MamanSpecialActionCode");
            AddForeignKey("Customs.ClaimsRelatedEntitiesSeizures", "SeizureFactorCode", "Customs.SeizureFactorTypes", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.ClaimsRelatedEntitiesSeizures", "SeizureFactorCode", "Customs.SeizureFactorTypes");
            DropIndex("Customs.DeclarationMamanSpecialActions", new[] { "MamanSpecialActionCode" });
            DropIndex("Customs.ClaimsRelatedEntitiesSeizures", new[] { "SeizureFactorCode" });
            DropPrimaryKey("Customs.DeclarationMamanSpecialActions");
            AlterColumn("Customs.DeclarationMamanSpecialActions", "MamanSpecialActionCode", c => c.String(maxLength: 2, unicode: false));
            AddPrimaryKey("Customs.DeclarationMamanSpecialActions", "DeclarationId");
            CreateIndex("Customs.DeclarationMamanSpecialActions", "MamanSpecialActionCode");
        }
    }
}
