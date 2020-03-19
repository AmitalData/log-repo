namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSupportMailBoxToTicketTable_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "SupportMailboxId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.Tickets", "SupportMailboxId");
            AddForeignKey("dbo.Tickets", "SupportMailboxId", "dbo.SupportMailboxes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "SupportMailboxId", "dbo.SupportMailboxes");
            DropIndex("dbo.Tickets", new[] { "SupportMailboxId" });
            DropColumn("dbo.Tickets", "SupportMailboxId");
        }
    }
}
