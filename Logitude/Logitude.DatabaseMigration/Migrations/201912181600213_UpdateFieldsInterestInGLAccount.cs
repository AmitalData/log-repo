namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFieldsInterestInGLAccount : DbMigration
    {
        public override void Up()
        {
            Sql("update GLAccounts set InterestCalculationStartDate = null  where InterestCalculationStartDate = '1900-01-01 00:00:00.000'");
            Sql("update GLAccounts set MinimumInterestInvoiceBilling = null where MinimumInterestInvoiceBilling = 0");
        }

        public override void Down()
        {

        }
    }
}
