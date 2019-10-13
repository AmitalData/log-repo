namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransportModeCodeToTariffTypesTable_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TariffTypes", "TransportModeCode", c => c.String(maxLength: 1, fixedLength: true, unicode: false));
            CreateIndex("dbo.TariffTypes", "TransportModeCode");
            AddForeignKey("dbo.TariffTypes", "TransportModeCode", "dbo.TransportModes", "Id");
            Sql("Update TariffTypes set TransportModeCode='A' where Code = 'AFC' or Code ='ASC'");
            Sql("Update TariffTypes set TransportModeCode='O' where Code = 'OSC' or Code ='OLC'");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TariffTypes", "TransportModeCode", "dbo.TransportModes");
            DropIndex("dbo.TariffTypes", new[] { "TransportModeCode" });
            DropColumn("dbo.TariffTypes", "TransportModeCode");
        }
    }
}
