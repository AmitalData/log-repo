namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParentVersionIdToTariffVersion_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TariffVersions", "ParentVersionId", c => c.String(maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TariffVersions", "ParentVersionId");
        }
    }
}
