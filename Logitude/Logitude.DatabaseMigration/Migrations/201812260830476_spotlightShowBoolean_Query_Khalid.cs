namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class spotlightShowBoolean_Query_Khalid : DbMigration
    {
        public override void Up()
        {        
            AddColumn("dbo.Queries", "SpotlightModeActivated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Queries", "SpotlightModeActivated");
        }
    }
}
