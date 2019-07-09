namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAllOtherPortsToTariffLine_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TariffLines", "IsFromAllOtherPorts", c => c.Boolean(nullable: false));
            AddColumn("dbo.TariffLines", "IsToAllOtherPorts", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TariffLines", "IsToAllOtherPorts");
            DropColumn("dbo.TariffLines", "IsFromAllOtherPorts");
        }
    }
}
