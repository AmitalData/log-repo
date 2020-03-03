namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamDBMigrationFixes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contacts", "ExternalId", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Followers", "CancelledDate", c => c.DateTime());
            AlterColumn("dbo.ObjectTables", "CodeField", c => c.String(maxLength: 60, unicode: false));
            AlterColumn("dbo.ObjectTables", "SplitComponentPath", c => c.String());
            DropIndex("dbo.Tenants", new[] { "PasswordPolicyCode" });
            AlterColumn("dbo.Tenants", "PasswordPolicyCode", c => c.String(maxLength: 4, unicode: false));
            CreateIndex("dbo.Tenants", "PasswordPolicyCode");
        }
        
        public override void Down()
        {
        }
    }
}
