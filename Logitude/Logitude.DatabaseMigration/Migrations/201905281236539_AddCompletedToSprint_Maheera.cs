namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompletedToSprint_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sprints", "IsCompleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sprints", "IsCompleted");
        }
    }
}
