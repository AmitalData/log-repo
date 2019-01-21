namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationCourierSpecialActionStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.DeclarationCourierStatuses", "SpecialActionStatus", c => c.String(maxLength: 1, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("Customs.DeclarationCourierStatuses", "SpecialActionStatus");
        }
    }
}
