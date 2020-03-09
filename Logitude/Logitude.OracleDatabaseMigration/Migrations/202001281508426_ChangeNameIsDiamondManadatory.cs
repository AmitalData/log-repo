namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeNameIsDiamondManadatory : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.CustomDocumentTypes", "IsDiamondManadatory", c => c.Boolean(nullable: false));
            DropColumn("Customs.CustomDocumentTypes", "IsManadatory");
        }
        
        public override void Down()
        {
            AddColumn("Customs.CustomDocumentTypes", "IsManadatory", c => c.Boolean(nullable: false));
            DropColumn("Customs.CustomDocumentTypes", "IsDiamondManadatory");
        }
    }
}
