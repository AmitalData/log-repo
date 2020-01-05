namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_InterestReportLineByDate_UpdateFieldsFormat_of_LineNumber : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.InterestReportLinesByDates", "LineNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InterestReportLinesByDates", "LineNumber", c => c.Decimal(nullable: false, precision: 6, scale: 0));
        }
    }
}
