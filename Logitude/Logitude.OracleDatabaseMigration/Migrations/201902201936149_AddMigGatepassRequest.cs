namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigGatepassRequest : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("Customs.GatepassRequests");
            AlterColumn("Customs.GatepassRequests", "GatepassNumber", c => c.Int());
            AddPrimaryKey("Customs.GatepassRequests", "MasterCourierId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("Customs.GatepassRequests");
            AlterColumn("Customs.GatepassRequests", "GatepassNumber", c => c.Int(nullable: false));
            AddPrimaryKey("Customs.GatepassRequests", new[] { "MasterCourierId", "GatepassNumber" });
        }
    }
}
