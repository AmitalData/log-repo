namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAGGridOptionsXMLToBiReportTable_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BIReports", "AGGridOptionsXML", c => c.String());            
        }
        
        public override void Down()
        {
            DropColumn("dbo.BIReports", "AGGridOptionsXML");
        }
    }
}
