namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_ShouldntBeExecuted : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GLAccounts", "DisplayNumber", c => c.String(nullable: false, maxLength: 15, unicode: false));
            CreateIndex("dbo.Tenants", "ChargeableWeightUnitCode");
            AddForeignKey("dbo.Tenants", "ChargeableWeightUnitCode", "dbo.WeightUnits", "Code");

        }

        public override void Down()
        {

            DropForeignKey("dbo.Tenants", "ChargeableWeightUnitCode", "dbo.WeightUnits");
            DropIndex("dbo.Tenants", new[] { "ChargeableWeightUnitCode" });
            AlterColumn("dbo.GLAccounts", "DisplayNumber", c => c.String(maxLength: 15, unicode: false));

        }
    }
}
