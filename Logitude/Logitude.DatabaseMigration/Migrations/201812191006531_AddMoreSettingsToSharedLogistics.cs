namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreSettingsToSharedLogistics : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SharedLogisticsSettings", "IsShipperShared", c => c.Boolean(nullable: false));
            AddColumn("dbo.SharedLogisticsSettings", "IsConsigneeShared", c => c.Boolean(nullable: false));
            Sql("update SharedLogisticsSettings set IsShipperShared = 1, IsConsigneeShared = 1");
        }
        
        public override void Down()
        {
            DropColumn("dbo.SharedLogisticsSettings", "IsConsigneeShared");
            DropColumn("dbo.SharedLogisticsSettings", "IsShipperShared");
        }
    }
}
