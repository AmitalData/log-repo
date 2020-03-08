namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddIsManadatory_DeclarationType : DbMigration
    {
        public override void Up()
        {


            AddColumn("Customs.Declarations", "DeclarationType", c => c.String(maxLength: 1, unicode: false));
            AddColumn("Customs.CustomDocumentTypes", "IsManadatory", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {

            DropColumn("Customs.CustomDocumentTypes", "IsManadatory");
            DropColumn("Customs.Declarations", "DeclarationType");

        }
    }
}
