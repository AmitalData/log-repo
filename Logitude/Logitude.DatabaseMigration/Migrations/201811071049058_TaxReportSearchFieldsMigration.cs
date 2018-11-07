namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxReportSearchFieldsMigration : DbMigration
    {
        public override void Up()
        {
            Sql("Update TaxReports set SearchFields = isnull(TaxReportNumber, '') + ',' +isnull(VatNumber, '')");
        }
        
        public override void Down()
        {
        }
    }
}
