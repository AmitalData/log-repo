namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Abed_FixReportGroupId : DbMigration
    {
        public override void Up()
        {
            //DropIndex("dbo.Reports", new[] { "ReportGroupId" });
            //AlterColumn("dbo.Reports", "ReportGroupId", c => c.String(maxLength: 15, unicode: false));
            //CreateIndex("dbo.Reports", "ReportGroupId");
        }
        
        public override void Down()
        {
            //DropIndex("dbo.Reports", new[] { "ReportGroupId" });
            //AlterColumn("dbo.Reports", "ReportGroupId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //CreateIndex("dbo.Reports", "ReportGroupId");
        }
    }
}
