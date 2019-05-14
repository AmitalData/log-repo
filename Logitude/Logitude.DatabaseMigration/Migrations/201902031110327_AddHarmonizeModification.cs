namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHarmonizeModification : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HarmonizeCodes", "SearchFields", c => c.String(nullable: false, maxLength: 4000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HarmonizeCodes", "SearchFields", c => c.String(nullable: false, maxLength: 1000));
        }
    }
}
