namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyIsRequiredPropertyOfCostMeasurementIdFieldOfQuoteChargeTable_Maheera : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.QuoteCharges", "IX_FK_CostQuoteChargesMeasurement");
            AlterColumn("dbo.QuoteCharges", "CostMeasurementId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            CreateIndex("dbo.QuoteCharges", "CostMeasurementId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.QuoteCharges", new[] { "CostMeasurementId" });
            AlterColumn("dbo.QuoteCharges", "CostMeasurementId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.QuoteCharges", "CostMeasurementId");
        }
    }
}
