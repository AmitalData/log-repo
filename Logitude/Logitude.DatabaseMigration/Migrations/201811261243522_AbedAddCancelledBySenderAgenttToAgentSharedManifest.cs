namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddCancelledBySenderAgenttToAgentSharedManifest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AgentSharedManifests", "CancelledBySenderAgent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AgentSharedManifests", "CancelledBySenderAgent");
        }
    }
}
