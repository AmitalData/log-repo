namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxWithholdingDeductionTypeMigration : DbMigration
    {
        public override void Up()
        {

            Sql("update glaccounts set DeductionFileTypeId=null");
            Sql("  delete from WithholdingTaxDeductionTypes where  Tenant <> 0");

        }
        
        public override void Down()
        {
        }
    }
}
