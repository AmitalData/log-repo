namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSearchFieldsToSLAHeadersAndSupportMailBoxes_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SupportMailboxes", "SearchFields", c => c.String(maxLength: 1000));
            AddColumn("dbo.SLAHeaders", "SearchFields", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SLAHeaders", "SearchFields");
            DropColumn("dbo.SupportMailboxes", "SearchFields");
        }
    }
}
