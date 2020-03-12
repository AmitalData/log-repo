namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTMPersonalAccessTokenFieldToSettings_Maheera : DbMigration
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
