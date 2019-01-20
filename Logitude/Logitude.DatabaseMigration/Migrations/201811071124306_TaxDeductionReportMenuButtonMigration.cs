namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxDeductionReportMenuButtonMigration : DbMigration
    {
        public override void Up()
        {
            Sql(" delete from MenuButtons where FeatureId = (select Id from Features where NameTextCodeId = (select ID from TextCodes where Code = 'taxDeductionreport.features.downloadtextfile'))");
            Sql("delete from PackageFeatures where FeatureId = (select Id from Features where NameTextCodeId = (select ID from TextCodes where Code = 'taxDeductionreport.features.downloadtextfile'))");
            Sql("delete from RoleFeatures where FeatureId = (select Id from Features where NameTextCodeId = (select ID from TextCodes where Code = 'taxDeductionreport.features.downloadtextfile'))");
            Sql("delete from Features where NameTextCodeId = (select ID from TextCodes where Code = 'taxDeductionreport.features.downloadtextfile')");
            Sql("delete from TextCodes where Code = 'taxDeductionreport.features.downloadtextfile'");
            Sql("delete from TextCodes where Code = 'taxDeductionreport.B.downloadtextfile'");




        }

        public override void Down()
        {
        }
    }
}
