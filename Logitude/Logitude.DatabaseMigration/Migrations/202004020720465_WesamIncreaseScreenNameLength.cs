namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamIncreaseScreenNameLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Screens", "Name", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Screens", "Name", c => c.String(maxLength: 40, unicode: false));
        }
    }
}
