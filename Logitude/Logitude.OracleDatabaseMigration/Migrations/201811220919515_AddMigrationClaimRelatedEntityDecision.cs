namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationClaimRelatedEntityDecision : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.ClaimsRelatedEntities", "DecisionCode", c => c.String(maxLength: 2, unicode: false));
            AddColumn("Customs.ClaimsRelatedEntities", "DecisionNote", c => c.String(maxLength: 256));
            AddColumn("Customs.ClaimsRelatedEntities", "EilatVatRefoundDecision", c => c.String(maxLength: 256));
            AddColumn("Customs.ClaimsRelatedEntities", "DepositingAmount", c => c.Decimal(precision: 16, scale: 2));
            AddColumn("Customs.ClaimsRelatedEntities", "RefundAmount", c => c.Decimal(precision: 16, scale: 2));
            CreateIndex("Customs.ClaimsRelatedEntities", "DecisionCode");
            AddForeignKey("Customs.ClaimsRelatedEntities", "DecisionCode", "Customs.DecisionTypes", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.ClaimsRelatedEntities", "DecisionCode", "Customs.DecisionTypes");
            DropIndex("Customs.ClaimsRelatedEntities", new[] { "DecisionCode" });
            DropColumn("Customs.ClaimsRelatedEntities", "RefundAmount");
            DropColumn("Customs.ClaimsRelatedEntities", "DepositingAmount");
            DropColumn("Customs.ClaimsRelatedEntities", "EilatVatRefoundDecision");
            DropColumn("Customs.ClaimsRelatedEntities", "DecisionNote");
            DropColumn("Customs.ClaimsRelatedEntities", "DecisionCode");
        }
    }
}
