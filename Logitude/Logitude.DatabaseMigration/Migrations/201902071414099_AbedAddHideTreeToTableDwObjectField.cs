namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddHideTreeToTableDwObjectField : DbMigration
    {
        public override void Up()
        { 
            AddColumn("dbo.DWObjectFields", "HideTree", c => c.Boolean(nullable: false));
           
        }
        
        public override void Down()
        {
            DropColumn("dbo.DWObjectFields", "HideTree");
        }
    }
}
