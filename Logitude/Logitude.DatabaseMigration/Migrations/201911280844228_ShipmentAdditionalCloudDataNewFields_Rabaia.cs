namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShipmentAdditionalCloudDataNewFields_Rabaia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentAdditionalCloudDatas", "IsUserIDNumberRequired", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumberUpdateDate", c => c.DateTime());
            AddColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumberXMLData", c => c.String());
            AddColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumber", c => c.String(maxLength:35));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumber");
            DropColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumberXMLData");
            DropColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumberUpdateDate");
            DropColumn("dbo.ShipmentAdditionalCloudDatas", "IsUserIDNumberRequired");
        }
    }
}
