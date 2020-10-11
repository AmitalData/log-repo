namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSearchfieldTAutonomyKeyword : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.CustomsAutonomyKeywords", "SearchFields", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("Customs.CustomsAutonomyKeywords", "SearchFields");
        }
    }
}
