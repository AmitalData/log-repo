namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExcludeFromProratingToTMProject_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TMProjects", "ExcludeFromProrating", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TMProjects", "ExcludeFromProrating");
        }
    }
}
