namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddCaptchaKeyToContactPassword : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactPasswords", "CaptchaKey", c => c.String(maxLength: 40, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContactPasswords", "CaptchaKey");
        }
    }
}
