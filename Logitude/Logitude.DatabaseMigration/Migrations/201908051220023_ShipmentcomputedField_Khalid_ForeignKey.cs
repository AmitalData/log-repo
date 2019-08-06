namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShipmentcomputedField_Khalid_ForeignKey : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Shipments", "AgentComputed");
            AddForeignKey("dbo.Shipments", "AgentComputed", "dbo.Cards", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shipments", "AgentComputed", "dbo.Cards");
            DropIndex("dbo.Shipments", new[] { "AgentComputed" });
        }
    }
}
