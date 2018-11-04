namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExternalProjectnumberToTMProject_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TMProjects", "ExternalProjectNumber", c => c.String(maxLength: 10, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TMProjects", "ExternalProjectNumber");
        }
    }
}
