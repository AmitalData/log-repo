namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_AgentComputed_Shipments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "AgentComputed", c => c.String(maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "AgentComputed");
        }
    }
}
