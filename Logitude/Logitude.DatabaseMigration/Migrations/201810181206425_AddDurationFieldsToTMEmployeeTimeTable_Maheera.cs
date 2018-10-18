namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDurationFieldsToTMEmployeeTimeTable_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TMEmployeeTimes", "ProratedDuration", c => c.Double(nullable: false));
            AddColumn("dbo.TMEmployeeTimes", "FullDuration", c => c.Double(nullable: false));
            AddColumn("dbo.TMProjects", "IsProrated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TMProjects", "IsProrated");
            DropColumn("dbo.TMEmployeeTimes", "FullDuration");
            DropColumn("dbo.TMEmployeeTimes", "ProratedDuration");
        }
    }
}
