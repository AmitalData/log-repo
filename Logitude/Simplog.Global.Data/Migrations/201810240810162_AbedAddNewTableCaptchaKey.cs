namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddNewTableCaptchaKey : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CaptchaKeys",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 40, unicode: false),
                        Code = c.String(nullable: false, maxLength: 6, unicode: false),
                        CreateDate = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 70, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
          
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CaptchaKeys");
        }
    }
}
