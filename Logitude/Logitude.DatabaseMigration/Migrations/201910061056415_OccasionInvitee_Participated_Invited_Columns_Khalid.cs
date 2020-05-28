namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OccasionInvitee_Participated_Invited_Columns_Khalid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OccasionInvitees", "Invited", c => c.Boolean(nullable: false));
            AddColumn("dbo.OccasionInvitees", "Participated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OccasionInvitees", "Participated");
            DropColumn("dbo.OccasionInvitees", "Invited");
        }
    }
}
