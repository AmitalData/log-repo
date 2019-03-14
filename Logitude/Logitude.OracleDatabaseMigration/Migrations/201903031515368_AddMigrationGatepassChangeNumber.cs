namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationGatepassChangeNumber : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Customs.GatepassRequests", "GatepassNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Customs.GatepassRequests", "GatepassNumber", c => c.Int());
        }
    }
}
