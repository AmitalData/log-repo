namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContainerDefaultsToTariffSettings_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TariffSettings", "ContainerDefaults", c => c.String(maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TariffSettings", "ContainerDefaults");
        }
    }
}
