namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTarifflineUniqueKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TariffLines", "UniqueKey", c => c.String(maxLength: 100));
            AddColumn("dbo.TariffLines", "UniqueKeyText", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TariffLines", "UniqueKeyText");
            DropColumn("dbo.TariffLines", "UniqueKey");
        }
    }
}
