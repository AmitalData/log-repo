namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsAutomaticManifestSent_NoOfCourierHawb : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.CourierMasters", "NoOfCourierHawb", c => c.String(maxLength: 5, unicode: false));
            AddColumn("Customs.CourierMasters", "IsAutomaticManifestSent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Customs.CourierMasters", "IsAutomaticManifestSent");
            DropColumn("Customs.CourierMasters", "NoOfCourierHawb");
        }
    }
}
