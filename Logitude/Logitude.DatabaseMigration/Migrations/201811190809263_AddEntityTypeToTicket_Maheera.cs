namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEntityTypeToTicket_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "EntityType", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.Tickets", "EntityType");
            AddForeignKey("dbo.Tickets", "EntityType", "dbo.ObjectTables", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "EntityType", "dbo.ObjectTables");
            DropIndex("dbo.Tickets", new[] { "EntityType" });
            DropColumn("dbo.Tickets", "EntityType");
        }
    }
}
