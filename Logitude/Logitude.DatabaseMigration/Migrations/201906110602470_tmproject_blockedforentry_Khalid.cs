namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tmproject_blockedforentry_Khalid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TMProjects", "BlockedForDataEntry", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TMProjects", "BlockedForDataEntry");
        }
    }
}
