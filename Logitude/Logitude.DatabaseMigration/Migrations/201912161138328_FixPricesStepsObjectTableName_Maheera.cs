namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixPricesStepsObjectTableName_Maheera : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PricesSteps", newName: "PriceSteps");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.PriceSteps", newName: "PricesSteps");
        }
    }
}
