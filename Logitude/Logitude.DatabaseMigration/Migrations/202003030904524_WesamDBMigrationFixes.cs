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
        }
        
        public override void Down()
        {
        }
    }
}
