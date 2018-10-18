namespace Logitude.SystemLogs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class abedaddColumViaToTableContactActivityLog : DbMigration
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
