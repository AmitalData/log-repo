namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeParentVersionTypeInTariffVersionTable_Maheera : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TariffVersions", name: "ParentVersionId", newName: "ParentVersionNumber");
            AlterColumn("dbo.TariffVersions", "ParentVersionNumber", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.TariffVersions", "ParentVersionNumber");
        }
    }
}
