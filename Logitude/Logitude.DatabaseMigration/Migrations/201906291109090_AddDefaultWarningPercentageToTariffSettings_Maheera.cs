namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDefaultWarningPercentageToTariffSettings_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TariffSettings", "DefaultWarningPercentage", c => c.Double());
            DropColumn("dbo.Tenants", "DefaultWarningPercentage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tenants", "DefaultWarningPercentage", c => c.Double());
            DropColumn("dbo.TariffSettings", "DefaultWarningPercentage");
        }
    }
}
