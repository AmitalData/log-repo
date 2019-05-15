namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameTarifflineUniqueKey : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TariffLines", name: "UniqueKey", newName: "LineUniqueKey");
            RenameColumn(table: "dbo.TariffLines", name: "UniqueKeyText", newName: "LineUniqueKeyText");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.TariffLines", name: "LineUniqueKey", newName: "UniqueKey");
            RenameColumn(table: "dbo.TariffLines", name: "LineUniqueKeyText", newName: "UniqueKeyText");
        }
    }
}
