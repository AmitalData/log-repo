namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBIReportType_Maheera : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BIReportsTypes", "Name", c => c.String(maxLength: 60, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BIReportsTypes", "Name", c => c.String(maxLength: 3, unicode: false));
        }
    }
}
