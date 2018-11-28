namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddNewFieldActivityToCaptchaKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CaptchaKeys", "Activity", c => c.String(maxLength: 70, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CaptchaKeys", "Activity");
        }
    }
}
