namespace Logitude.Global.OracleMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingglobal20042020 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "TMPersonalAccessToken", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "TMPersonalAccessToken");
        }
    }
}
