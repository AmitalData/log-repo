namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddToggleCodeToFeature_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Features", "ToggleCode", c => c.String(maxLength: 3, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Features", "ToggleCode");
        }
    }
}
