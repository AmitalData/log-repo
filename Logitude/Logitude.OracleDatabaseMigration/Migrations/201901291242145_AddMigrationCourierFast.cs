namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationCourierFast : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.DeclarationCourierStatuses", "FastIndividualProcessCode", c => c.String(maxLength: 3, unicode: false));
            AddColumn("Customs.DeclarationCourierStatuses", "ManualProcessCode", c => c.String(maxLength: 3, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("Customs.DeclarationCourierStatuses", "ManualProcessCode");
            DropColumn("Customs.DeclarationCourierStatuses", "FastIndividualProcessCode");
        }
    }
}
