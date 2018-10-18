namespace Logitude.SystemLogs.OracleMigratrion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newmigrations191115 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactActivityLogs", "Via", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContactActivityLogs", "Via");
        }
    }
}
