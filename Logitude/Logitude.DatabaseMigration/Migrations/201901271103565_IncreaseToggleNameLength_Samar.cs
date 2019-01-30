namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreaseToggleNameLength_Samar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Toggles", "Name", c => c.String(nullable: false, maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Toggles", "Name", c => c.String(nullable: false, maxLength: 3, unicode: false));
        }
    }
}
