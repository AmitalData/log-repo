namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTariffNumberToTariff_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tariffs", "TariffNumber", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tariffs", "TariffNumber");
        }
    }
}
