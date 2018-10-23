namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyTheNameFieldOfSprint_Maheera : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sprints", "Name", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sprints", "Name", c => c.String(maxLength: 100));
        }
    }
}
