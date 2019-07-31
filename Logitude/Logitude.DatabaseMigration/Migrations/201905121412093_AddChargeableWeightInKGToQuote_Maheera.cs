namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChargeableWeightInKGToQuote_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quotes", "ChargeableWeightInKG", c => c.Double());
            Sql("update Quotes set ChargeableWeightInKG = round(ChargeableWeight, 3) where ChargeableWeight is not null and ChargeableWeightUnitCode = 'KG'");
            Sql("update Quotes set ChargeableWeightInKG = round(ChargeableWeight * 0.45359237, 3) where ChargeableWeight is not null and ChargeableWeightUnitCode = 'LB'");
            Sql("update Quotes set ChargeableWeightInKG = round(ChargeableWeight * 1000, 3) where ChargeableWeight is not null and ChargeableWeightUnitCode = 'MT'");
        }

        public override void Down()
        {
            DropColumn("dbo.Quotes", "ChargeableWeightInKG");
        }
    }
}
