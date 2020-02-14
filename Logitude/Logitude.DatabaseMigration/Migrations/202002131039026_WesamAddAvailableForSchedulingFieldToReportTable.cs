namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddAvailableForSchedulingFieldToReportTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reports", "AvailableForScheduling", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reports", "AvailableForScheduling");
        }
    }
}
