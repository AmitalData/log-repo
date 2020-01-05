namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Update_LocalName_In_InterestReportStatues : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InterestReportStatuses", "LocalName", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InterestReportStatuses", "LocalName", c => c.String(nullable: false, maxLength: 30, unicode: false));
        }
    }
}
