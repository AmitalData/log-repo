namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Field_TariffLine_DestinationPortId2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TariffLines", "DestinationPortId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.TariffLines", "OriginPortId");
            CreateIndex("dbo.TariffLines", "DestinationPortId");
            AddForeignKey("dbo.TariffLines", "DestinationPortId", "dbo.Ports", "Id");
            AddForeignKey("dbo.TariffLines", "OriginPortId", "dbo.Ports", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TariffLines", "OriginPortId", "dbo.Ports");
            DropForeignKey("dbo.TariffLines", "DestinationPortId", "dbo.Ports");
            DropIndex("dbo.TariffLines", new[] { "DestinationPortId" });
            DropIndex("dbo.TariffLines", new[] { "OriginPortId" });
            DropColumn("dbo.TariffLines", "DestinationPortId");
        }
    }
}
