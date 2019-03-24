namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OpenFormatReportXMLFieldMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OpenFormatReports", "PDFRerportXML", c => c.String());
           // AddColumn("dbo.TMBudgets", "Inactive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TMBudgets", "Inactive");
            DropColumn("dbo.OpenFormatReports", "PDFRerportXML");
        }
    }
}
