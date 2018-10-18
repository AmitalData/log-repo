namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddSessionTimeoutToTenantLoginPolicy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TenantLoginPolicies", "SessionTimeout", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TenantLoginPolicies", "SessionTimeout");
        }
    }
}
