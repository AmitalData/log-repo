namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewReleaseToolTipForUser_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ShowNewReleaseToolTip", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ShowNewReleaseToolTip");
        }
    }
}
