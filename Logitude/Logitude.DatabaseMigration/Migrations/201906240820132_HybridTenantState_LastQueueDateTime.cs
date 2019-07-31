namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HybridTenantState_LastQueueDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HybridTenantStates", "LastQueueDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HybridTenantStates", "LastQueueDateTime");
        }
    }
}
