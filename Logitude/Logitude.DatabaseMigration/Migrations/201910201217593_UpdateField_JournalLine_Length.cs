namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateField_JournalLine_Length : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JournalLines", "Notes", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JournalLines", "Notes", c => c.String(maxLength: 60));
        }
    }
}
