namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_InterestReportHeaderScreen : DbMigration
    {
        public override void Up()
        {
            Sql("delete from ScreenFields where ScreenCode='InterestReport.HeaderScreen'");
            Sql("delete from Screens where Code = 'InterestReport.HeaderScreen'");

        }

        public override void Down()
        {
         }
    }
}
