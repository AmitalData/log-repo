namespace Logitude.SystemLogs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIptoErrorlog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ErrorLogs", "IP", c => c.String(maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ErrorLogs", "IP");
        }
    }
}
