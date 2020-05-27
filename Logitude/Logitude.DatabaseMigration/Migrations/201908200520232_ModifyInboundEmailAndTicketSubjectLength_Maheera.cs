namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyInboundEmailAndTicketSubjectLength_Maheera : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tickets", "Subject", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.InboundEmailLines", "Subject", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InboundEmailLines", "Subject", c => c.String(maxLength: 100));
            AlterColumn("dbo.Tickets", "Subject", c => c.String(nullable: false, maxLength: 250));
        }
    }
}
