namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFieldNameInTableSessionPolicies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SessionPolicies", "WebTokenLifeTimeInMinutes", c => c.Int(nullable: false));
            AddColumn("dbo.SessionPolicies", "WebTokenExpirationWarningInMinutes", c => c.Int(nullable: false));
            DropColumn("dbo.SessionPolicies", "WebTokenLifeTime");
            DropColumn("dbo.SessionPolicies", "WebTokenExpirationWarning");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SessionPolicies", "WebTokenExpirationWarning", c => c.Int(nullable: false));
            AddColumn("dbo.SessionPolicies", "WebTokenLifeTime", c => c.Int(nullable: false));
            DropColumn("dbo.SessionPolicies", "WebTokenExpirationWarningInMinutes");
            DropColumn("dbo.SessionPolicies", "WebTokenLifeTimeInMinutes");
        }
    }
}
