namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddFieldIsUsedOnTableCaptchaKey : DbMigration
    {

        public override void Up()
        {
            AddColumn("dbo.CaptchaKeys", "IsUsed", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.CaptchaKeys", "IsUsed");
        }
    }
}
