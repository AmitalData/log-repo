namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTariffIdToAllInChargesTable_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TariffVersionAllInCharges", "TariffId", c => c.String(nullable: false, maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TariffVersionAllInCharges", "TariffId");
        }
    }
}
