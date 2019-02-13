namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewField_EnableFullscreenTextBox : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObjectFields", "EnableFullscreenTextBox", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ObjectFields", "EnableFullscreenTextBox");
        }
    }
}
