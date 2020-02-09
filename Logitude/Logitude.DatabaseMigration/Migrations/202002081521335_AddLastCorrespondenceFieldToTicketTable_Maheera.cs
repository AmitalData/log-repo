namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastCorrespondenceFieldToTicketTable_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "LastCorrespondence", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "LastCorrespondence");
        }
    }
}
