namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddShortTextCodeCodeToTipsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tips", "ShortTextCodeCode", c => c.String(nullable: true, maxLength: 100, unicode: false));
            Sql(@"update Tips set ShortTextCodeCode = (select TextCodes.Code from TextCodes where id = Tips.ShortTextCode)");
            AlterColumn("dbo.Tips", "ShortTextCodeCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tips", "ShortTextCodeCode");
        }
    }
}
