namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesHazardousSubstances1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Customs.HazardousSubstances", "EnglishName", c => c.String(maxLength: 300, unicode: false));
            AlterColumn("Customs.HazardousSubstances", "LocalName", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("Customs.HazardousSubstances", "LocalName", c => c.String(maxLength: 100));
            AlterColumn("Customs.HazardousSubstances", "EnglishName", c => c.String(maxLength: 100, unicode: false));
        }
    }
}
