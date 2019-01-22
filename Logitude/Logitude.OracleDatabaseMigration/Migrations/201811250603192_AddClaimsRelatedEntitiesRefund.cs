namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClaimsRelatedEntitiesRefund : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customs.ClaimsRelatedEntitiesRefunds",
                c => new
                    {
                        ClaimId = c.String(nullable: false, maxLength: 15, unicode: false),
                        CounterKey = c.Int(nullable: false),
                        RefundQuntityLineNo = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        InvoiceNumber = c.Int(),
                        SequenceNumeric = c.Int(),
                        RefundQuntity = c.Decimal(precision: 16, scale: 6),
                    })
                .PrimaryKey(t => new { t.ClaimId, t.CounterKey, t.RefundQuntityLineNo })
                .ForeignKey("Customs.ClaimsRelatedEntities", t => new { t.ClaimId, t.CounterKey })
                .Index(t => new { t.ClaimId, t.CounterKey });
            
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.ClaimsRelatedEntitiesRefunds", new[] { "ClaimId", "CounterKey" }, "Customs.ClaimsRelatedEntities");
            DropIndex("Customs.ClaimsRelatedEntitiesRefunds", new[] { "ClaimId", "CounterKey" });
            DropTable("Customs.ClaimsRelatedEntitiesRefunds");
        }
    }
}
