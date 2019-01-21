namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClaimsRelatedEntitiesSeizure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.ClaimsRelatedEntitiesSeizures",
                c => new
                    {
                        ClaimId = c.String(nullable: false, maxLength: 15, unicode: false),
                        CounterKey = c.Int(nullable: false),
                        SeizureLinoNo = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        SeizureFactorCode = c.String(maxLength: 2, unicode: false),
                        SeizureMethodCode = c.String(maxLength: 2, unicode: false),
                        SeizureAmount = c.Decimal(precision: 16, scale: 2),
                    })
                .PrimaryKey(t => new { t.ClaimId, t.CounterKey, t.SeizureLinoNo })
                .ForeignKey("Customs.ClaimsRelatedEntities", t => new { t.ClaimId, t.CounterKey })
                .ForeignKey("Customs.SeizureMethodTypes", t => t.SeizureMethodCode)
                .Index(t => new { t.ClaimId, t.CounterKey })
                .Index(t => t.SeizureMethodCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.ClaimsRelatedEntitiesSeizures", "SeizureMethodCode", "Customs.SeizureMethodTypes");
            DropForeignKey("Customs.ClaimsRelatedEntitiesSeizures", new[] { "ClaimId", "CounterKey" }, "Customs.ClaimsRelatedEntities");
            DropIndex("Customs.ClaimsRelatedEntitiesSeizures", new[] { "SeizureMethodCode" });
            DropIndex("Customs.ClaimsRelatedEntitiesSeizures", new[] { "ClaimId", "CounterKey" });
            DropTable("Customs.ClaimsRelatedEntitiesSeizures");
        }
    }
}
