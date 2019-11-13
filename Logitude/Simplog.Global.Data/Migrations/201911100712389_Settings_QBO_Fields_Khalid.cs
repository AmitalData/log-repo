namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Settings_QBO_Fields_Khalid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "QBOOAuthDefault", c => c.Int(nullable: false));
            AddColumn("dbo.Settings", "QBOClientID", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Settings", "QBOClientSecret", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "QBOClientSecret");
            DropColumn("dbo.Settings", "QBOClientID");
            DropColumn("dbo.Settings", "QBOOAuthDefault");
        }
    }
}
