namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddLabelTextCodeCodeToMenuButtonsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuButtons", "LabelTextCodeCode", c => c.String(nullable: true, maxLength: 100, unicode: false));
            Sql(@"update MenuButtons set LabelTextCodeCode = (select TextCodes.Code from TextCodes where id = MenuButtons.LabelTextCodeId)");
            AlterColumn("dbo.MenuButtons", "LabelTextCodeCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuButtons", "LabelTextCodeCode");
        }
    }
}
