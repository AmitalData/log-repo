namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddTableInvalidEmailResetPassword : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InvalidEmailResetPasswords",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        IP = c.String(maxLength: 15, unicode: false),
                        CreateDate = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 70, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.CaptchaKeys", "IP", c => c.String(maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CaptchaKeys", "IP");
            DropTable("dbo.InvalidEmailResetPasswords");
        }
    }
}
