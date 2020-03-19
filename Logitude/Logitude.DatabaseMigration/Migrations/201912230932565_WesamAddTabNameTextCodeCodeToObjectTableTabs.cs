namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddTabNameTextCodeCodeToObjectTableTabs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObjectTableTabs", "TabNameTextCodeCode", c => c.String(nullable: true, maxLength: 100, unicode: false));
            Sql(@"update ObjectTableTabs set TabNameTextCodeCode = (select TextCodes.Code from TextCodes where id = ObjectTableTabs.TabNameTextCodeId)");
            AlterColumn("dbo.ObjectTableTabs", "TabNameTextCodeCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ObjectTableTabs", "TabNameTextCodeCode");
        }
    }
}
