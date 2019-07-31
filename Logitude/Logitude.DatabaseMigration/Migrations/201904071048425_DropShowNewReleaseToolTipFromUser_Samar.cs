namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropShowNewReleaseToolTipFromUser_Samar : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "ShowNewReleaseToolTip");
            Sql("delete from ObjectFields where FieldName = 'ShowNewReleaseToolTip'");
            Sql("delete from TextCodes where Code like '%ShowNewReleaseToolTip%'");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "ShowNewReleaseToolTip", c => c.Boolean(nullable: false));
        }
    }
}
