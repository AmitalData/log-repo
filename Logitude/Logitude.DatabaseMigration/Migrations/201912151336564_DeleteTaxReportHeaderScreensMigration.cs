namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteTaxReportHeaderScreensMigration : DbMigration
    {
        public override void Up()
        {

            Sql("delete from screenfields where screenid = (select id from screens where code='TaxReportTaxReportHeaderScreen' and objecttableid=(select ID from ObjectTables where Name='taxreport'))");
            Sql("delete from screens where code='TaxReportTaxReportHeaderScreen' and objecttableid=(select ID from ObjectTables where Name='taxreport')");
            Sql("delete from screenfields where screenid = (select id from screens where code='TaxReport.TaxReportHeaderScreen' and objecttableid=(select ID from ObjectTables where Name='taxreport'))");
            Sql("delete from screens where code='TaxReportTaxReportHeaderScreen' and objecttableid=(select ID from ObjectTables where Name='taxreport')");


        }

        public override void Down()
        {
        }
    }
}
