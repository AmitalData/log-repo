namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateField_Journal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Journals", "SearchFields", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Journals", "SearchFields", c => c.String(maxLength: 1000));
        }
    }
}
