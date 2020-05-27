namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_InterestReportLineByDate_Add_Field_LineNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InterestReportLinesByDates", "LineNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InterestReportLinesByDates", "LineNumber");
        }
    }
}
