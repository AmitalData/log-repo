namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationPhysicalCheckChassis : DbMigration
    {
        public override void Up()
        {
            DropIndex("Customs.PendingByKeywords", new[] { "CourierPendingReasonCode" });
            AddColumn("Customs.PhysicalChecks", "VehicleChassisNumber", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("Customs.PendingByKeywords", "CourierPendingReasonCode", c => c.String(maxLength: 4, unicode: false));
            CreateIndex("Customs.PendingByKeywords", "CourierPendingReasonCode");
        }
        
        public override void Down()
        {
            DropIndex("Customs.PendingByKeywords", new[] { "CourierPendingReasonCode" });
            AlterColumn("Customs.PendingByKeywords", "CourierPendingReasonCode", c => c.String(nullable: false, maxLength: 4, unicode: false));
            DropColumn("Customs.PhysicalChecks", "VehicleChassisNumber");
            CreateIndex("Customs.PendingByKeywords", "CourierPendingReasonCode");
        }
    }
}
