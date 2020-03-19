namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAirLCLStepPricesToTariffSettings_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TariffSettings", "AirDefaultStepsId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TariffSettings", "LCLDefaultStepsId", c => c.String(maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TariffSettings", "LCLDefaultStepsId");
            DropColumn("dbo.TariffSettings", "AirDefaultStepsId");
        }
    }
}
