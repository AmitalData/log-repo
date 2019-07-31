namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustExternalProjectNumber_Maheera : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TMProjects", "ExternalProjectNumber", c => c.String(maxLength: 25, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TMProjects", "ExternalProjectNumber", c => c.String(maxLength: 10, unicode: false));
        }
    }
}
