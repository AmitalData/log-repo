namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesHazardousSubstances : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.HazardousSubstances", "EnglishName", c => c.String(maxLength: 100, unicode: false));
            AddColumn("Customs.HazardousSubstances", "LocalName", c => c.String(maxLength: 100));
            AddColumn("Customs.HazardousSubstances", "Inactive", c => c.Boolean(nullable: false));
            DropColumn("Customs.HazardousSubstances", "Name");
        }
        
        public override void Down()
        {
            AddColumn("Customs.HazardousSubstances", "Name", c => c.String(maxLength: 100, unicode: false));
            DropColumn("Customs.HazardousSubstances", "Inactive");
            DropColumn("Customs.HazardousSubstances", "LocalName");
            DropColumn("Customs.HazardousSubstances", "EnglishName");
        }
    }
}
