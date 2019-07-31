namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterOccasionDates_Maheera : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Occasions", "EndDateTime", c => c.DateTime(nullable: true));
            AlterColumn("dbo.Occasions", "StartDateTime", c => c.DateTime(nullable: true));

        }

        public override void Down()
        {
            AlterColumn("dbo.Occasions", "EndDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Occasions", "StartDateTime", c => c.DateTime(nullable: false));
        }
    }
}
