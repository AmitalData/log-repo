namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDWCategoriesTables_Rabaia : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.Toggles", "Name", c => c.String(nullable: false, maxLength: 100, unicode: false));
            //DropColumn("dbo.DWObjectFields", "Category1");
            //DropColumn("dbo.DWObjectFields", "Category2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DWObjectFields", "Category2", c => c.String(maxLength: 150, unicode: false));
            AddColumn("dbo.DWObjectFields", "Category1", c => c.String(maxLength: 150, unicode: false));
            //AlterColumn("dbo.Toggles", "Name", c => c.String(nullable: false, maxLength: 3, unicode: false));
        }
    }
}
