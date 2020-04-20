namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JournalFieldsMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Journals", "DocumentDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Journals", "DueDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
           
            DropColumn("dbo.Journals", "DueDate");
            DropColumn("dbo.Journals", "DocumentDate");
           
        }
    }
}
