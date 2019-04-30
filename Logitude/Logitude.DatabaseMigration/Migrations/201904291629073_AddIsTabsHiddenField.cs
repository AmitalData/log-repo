namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsTabsHiddenField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObjectTables", "IsTabsHidden", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ObjectTables", "IsTabsHidden");
        }
    }
}
