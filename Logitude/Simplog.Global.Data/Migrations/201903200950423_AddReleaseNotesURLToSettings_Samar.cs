namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReleaseNotesURLToSettings_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "ReleaseNotesURL", c => c.String(maxLength: 600, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "ReleaseNotesURL");
        }
    }
}
