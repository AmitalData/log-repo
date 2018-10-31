namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNeedsProratingToTMEmployeeTime_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TMEmployeeTimes", "NeedsProrating", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TMEmployeeTimes", "NeedsProrating");
        }
    }
}
