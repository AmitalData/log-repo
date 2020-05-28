namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_SearchField_To_InterestReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InterestReports", "SearchFields", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InterestReports", "SearchFields");
        }
    }
}
