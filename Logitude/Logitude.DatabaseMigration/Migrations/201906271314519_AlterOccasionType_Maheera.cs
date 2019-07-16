namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterOccasionType_Maheera : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OccasionTypes", "Code", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AlterColumn("dbo.OccasionTypes", "Name", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OccasionTypes", "Name", c => c.String(maxLength: 100));
            AlterColumn("dbo.OccasionTypes", "Code", c => c.String(maxLength: 3, unicode: false));
        }
    }
}
