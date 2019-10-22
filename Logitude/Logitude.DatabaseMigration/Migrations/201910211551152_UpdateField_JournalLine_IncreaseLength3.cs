namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateField_JournalLine_IncreaseLength3 : DbMigration
    {
        public override void Up()
        {
            // take this changes at conflicts
            AlterColumn("dbo.JournalLines", "Notes", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JournalLines", "Notes", c => c.String(maxLength: 500));
        }
    }
}
