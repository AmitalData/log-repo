namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxReportSearchFieldMigration : DbMigration
    {
        public override void Up()
        {
            Sql("Update TaxReports set SearchFields = isnull(TaxReportNumber, '')");
        }
        
        public override void Down()
        {
        }
    }
}
