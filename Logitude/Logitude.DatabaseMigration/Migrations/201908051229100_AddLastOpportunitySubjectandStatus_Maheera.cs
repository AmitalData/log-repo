namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastOpportunitySubjectandStatus_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "LastOpportunitySubject", c => c.String(maxLength: 250));
            AddColumn("dbo.Customers", "LastOpportunityStatus", c => c.String(maxLength: 60, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "LastOpportunityStatus");
            DropColumn("dbo.Customers", "LastOpportunitySubject");
        }
    }
}
